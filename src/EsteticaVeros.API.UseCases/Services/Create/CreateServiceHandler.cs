using EsteticaVeros.API.Core.ServiceAggregate;
using EsteticaVeros.API.Core.ServiceAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Services.Create;

public class CreateServiceHandler(
  IRepository<Service> repository,
  ILogger<CreateServiceHandler> logger) 
  : ICommandHandler<CreateServiceCommand, Result<ServiceDTO>>
{
  public async ValueTask<Result<ServiceDTO>> Handle(
    CreateServiceCommand command,
    CancellationToken cancellationToken)
  {
    try
    {
      // Validar tipo de servicio
      var serviceType = ServiceType.FromValue(command.ServiceType);

      // Validaciones por tipo
      var validationResult = ValidateByType(command, serviceType);
      if (!validationResult.IsSuccess)
      {
        return validationResult;
      }

      // Generar service number
      var isPackage = serviceType == ServiceType.Package;
      var sequence = await GetNextSequenceNumber(isPackage, cancellationToken);
      var serviceNumber = ServiceNumber.Generate(sequence, isPackage);

      // Crear servicio según tipo
      Service service;

      if (serviceType == ServiceType.Simple)
      {
        service = new Service(
          serviceNumber,
          serviceType,
          command.CategoryId!.Value,
          command.Name,
          command.Duration,
          command.BasePrice,
          command.CreatedByUserId
        );
      }
      else
      {
        if (command.ChildServiceIds != null && command.ChildServiceIds.Any())
        {
          var childValidation = await ValidateChildServices(command.ChildServiceIds, cancellationToken);
          if (!childValidation.IsSuccess)
          {
            return childValidation;
          }
        }

        service = Service.CreateCompositeOrPackage(
          serviceNumber,
          serviceType,
          command.CategoryId,
          command.Name,
          command.Duration,
          command.BasePrice,
          command.CreatedByUserId
        );
      }

      // Update optional fields
      if (!string.IsNullOrWhiteSpace(command.Description) || 
          !string.IsNullOrWhiteSpace(command.ShortDescription))
      {
        service.UpdateBasicInfo(command.Name, command.Description, command.ShortDescription);
      }

      if (command.OriginalPrice.HasValue || command.DiscountPercentage.HasValue)
      {
        service.UpdatePricing(command.BasePrice, command.OriginalPrice, command.DiscountPercentage);
      }

      if (command.LoyaltyPointsEarned > 0)
      {
        service.UpdateLoyaltyPoints(command.LoyaltyPointsEarned);
      }

      if (command.RequiresDeposit)
      {
        service.UpdateDepositSettings(command.RequiresDeposit, command.DepositAmount);
      }

      if (command.IsFeatured || command.DisplayOrder > 0 || !string.IsNullOrWhiteSpace(command.ImageUrl))
      {
        service.UpdateDisplaySettings(command.IsFeatured, command.DisplayOrder, command.ImageUrl);
      }

      if (!string.IsNullOrWhiteSpace(command.PreparationInstructions) ||
          !string.IsNullOrWhiteSpace(command.Contraindications) ||
          !string.IsNullOrWhiteSpace(command.Notes))
      {
        service.UpdateInstructions(
          command.PreparationInstructions, 
          command.Contraindications, 
          command.Notes);
      }

      var createdService = await repository.AddAsync(service, cancellationToken);

      if (serviceType != ServiceType.Simple && 
        command.ChildServiceIds != null && 
        command.ChildServiceIds.Any())
      {
        for (int i = 0; i < command.ChildServiceIds.Count; i++)
        {
          createdService.AddServiceItem(command.ChildServiceIds[i], quantity: 1, displayOrder: i);
        }

        createdService.ValidateBusinessRules();

        await repository.UpdateAsync(createdService, cancellationToken);
      }

      logger.LogInformation(
        "Service created: {ServiceNumber} - {Name} (Type: {Type})",
        createdService.ServiceNumber,
        createdService.Name,
        createdService.ServiceType.DisplayName);

      return Result.Success(await MapToDTO(createdService, repository, cancellationToken));
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating service");
      return Result.Error(ex.Message);
    }
  }

  private Result<ServiceDTO> ValidateByType(CreateServiceCommand command, ServiceType serviceType)
  {
    if (serviceType == ServiceType.Simple)
    {
      if (!command.CategoryId.HasValue)
      {
        return Result.Error("Simple services must have a CategoryId");
      }

      if (command.ChildServiceIds != null && command.ChildServiceIds.Any())
      {
        return Result.Error("Simple services cannot have child services");
      }
    }

    if (serviceType == ServiceType.Composite)
    {
      if (!command.CategoryId.HasValue)
      {
        return Result.Error("Composite services must have a CategoryId");
      }

      if (command.ChildServiceIds == null || command.ChildServiceIds.Count < 2)
      {
        return Result.Error("Composite services must have at least 2 child services");
      }

      if (command.ChildServiceIds.Count > 10)
      {
        return Result.Error("Composite services cannot have more than 10 child services");
      }
    }

    if (serviceType == ServiceType.Package)
    {
      if (command.CategoryId.HasValue)
      {
        return Result.Error("Packages cannot have a single CategoryId");
      }

      if (command.ChildServiceIds == null || command.ChildServiceIds.Count < 2)
      {
        return Result.Error("Packages must have at least 2 child services");
      }

      if (command.ChildServiceIds.Count > 10)
      {
        return Result.Error("Packages cannot have more than 10 child services");
      }
    }

    return Result.Success();
  }

  private async Task<Result<ServiceDTO>> ValidateChildServices(
    List<int> childServiceIds,
    CancellationToken cancellationToken)
  {
    foreach (var childId in childServiceIds)
    {
      var spec = new ServiceByIdSpec(childId);
      var childService = await repository.FirstOrDefaultAsync(spec, cancellationToken);

      if (childService == null)
      {
        return Result.Error($"Child service with ID {childId} not found");
      }

      if (childService.ServiceType != ServiceType.Simple)
      {
        return Result.Error($"Service {childService.Name} (ID: {childId}) is not a Simple service. Only Simple services can be added to Composite/Package services.");
      }

      if (!childService.IsActive)
      {
        return Result.Error($"Service {childService.Name} (ID: {childId}) is not active");
      }
    }

    return Result.Success();
  }

  private async Task<int> GetNextSequenceNumber(bool isPackage, CancellationToken cancellationToken)
  {
    var services = await repository.ListAsync(cancellationToken);
    
    var prefix = isPackage ? "PKG" : "SRV";
    var highestSequence = services
      .Where(s => s.ServiceNumber.Value.StartsWith(prefix))
      .Select(s =>
      {
        var parts = s.ServiceNumber.Value.Split('-');
        return parts.Length == 2 && int.TryParse(parts[1], out var seq) ? seq : 0;
      })
      .DefaultIfEmpty(0)
      .Max();

    return highestSequence + 1;
  }

  private static async Task<ServiceDTO> MapToDTO(
    Service service,
    IRepository<Service> repository,
    CancellationToken cancellationToken)
  {
    List<ServiceItemDTO>? items = null;

    if (service.ServiceItems.Any())
    {
      items = new List<ServiceItemDTO>();
      
      foreach (var item in service.ServiceItems)
      {
        var childSpec = new ServiceByIdSpec(item.ChildServiceId);
        var childService = await repository.FirstOrDefaultAsync(childSpec, cancellationToken);

        items.Add(new ServiceItemDTO(
          item.Id,
          item.ChildServiceId,
          childService?.Name ?? "Unknown",
          item.Quantity,
          item.DisplayOrder,
          item.Notes
        ));
      }
    }

    return new ServiceDTO(
      service.Id,
      service.ServiceNumber.Value,
      service.ServiceType.Value,
      service.ServiceType.DisplayName,
      service.CategoryId,
      service.Name,
      service.Description,
      service.ShortDescription,
      service.Duration,
      service.BasePrice,
      service.OriginalPrice,
      service.DiscountPercentage,
      service.LoyaltyPointsEarned,
      service.RequiresDeposit,
      service.DepositAmount,
      service.IsActive,
      service.IsFeatured,
      service.DisplayOrder,
      service.ImageUrl,
      service.Notes,
      service.PreparationInstructions,
      service.Contraindications,
      service.CreatedAt,
      service.UpdatedAt,
      items
    );
  }
}