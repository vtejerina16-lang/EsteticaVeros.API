using EsteticaVeros.API.Core.ServiceAggregate;
using EsteticaVeros.API.Core.ServiceAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Services.Update;

public class UpdateServiceHandler(
  IRepository<Service> repository,
  ILogger<UpdateServiceHandler> logger) 
  : ICommandHandler<UpdateServiceCommand, Result<ServiceDTO>>
{
  public async ValueTask<Result<ServiceDTO>> Handle(
    UpdateServiceCommand command,
    CancellationToken cancellationToken)
  {
    var spec = new ServiceByIdSpec(command.ServiceId, includeItems: true);
    var service = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (service == null)
    {
      return Result.NotFound();
    }

    // Update basic info
    service.UpdateBasicInfo(command.Name, command.Description, command.ShortDescription);
    
    // Update category
    service.UpdateCategory(command.CategoryId);

    // Update pricing
    service.UpdatePricing(command.BasePrice, command.OriginalPrice, command.DiscountPercentage);

    // Update duration
    service.UpdateDuration(command.Duration);

    // Update loyalty points
    service.UpdateLoyaltyPoints(command.LoyaltyPointsEarned);

    // Update deposit settings
    service.UpdateDepositSettings(command.RequiresDeposit, command.DepositAmount);

    // Update display settings
    service.UpdateDisplaySettings(command.IsFeatured, command.DisplayOrder, command.ImageUrl);

    // Update instructions
    service.UpdateInstructions(
      command.PreparationInstructions, 
      command.Contraindications, 
      command.Notes);

    // Update active status
    if (command.IsActive && !service.IsActive)
    {
      service.Activate();
    }
    else if (!command.IsActive && service.IsActive)
    {
      service.Deactivate();
    }

    // Update service items (solo para Composite/Package)
    if (service.ServiceType != ServiceType.Simple && command.ChildServiceIds != null)
    {
      var updateItemsResult = await UpdateServiceItems(service, command.ChildServiceIds, cancellationToken);
      if (!updateItemsResult.IsSuccess)
      {
        return updateItemsResult;
      }
    }

    // Validar reglas de negocio
    try
    {
      service.ValidateBusinessRules();
    }
    catch (Exception ex)
    {
      return Result.Error(ex.Message);
    }

    await repository.UpdateAsync(service, cancellationToken);

    logger.LogInformation(
      "Service updated: {ServiceNumber} - {Name}",
      service.ServiceNumber,
      service.Name);

    return Result.Success(await MapToDTO(service, repository, cancellationToken));
  }

  private async Task<Result<ServiceDTO>> UpdateServiceItems(
    Service service,
    List<int> newChildServiceIds,
    CancellationToken cancellationToken)
  {
    // Validar que los child services existan y sean Simple
    foreach (var childId in newChildServiceIds)
    {
      var spec = new ServiceByIdSpec(childId);
      var childService = await repository.FirstOrDefaultAsync(spec, cancellationToken);

      if (childService == null)
      {
        return Result.Error($"Child service with ID {childId} not found");
      }

      if (childService.ServiceType != ServiceType.Simple)
      {
        return Result.Error($"Service {childService.Name} is not a Simple service");
      }

      if (!childService.IsActive)
      {
        return Result.Error($"Service {childService.Name} is not active");
      }
    }

    // Remover items que ya no están en la nueva lista
    var currentChildIds = service.ServiceItems.Select(si => si.ChildServiceId).ToList();
    var itemsToRemove = currentChildIds.Except(newChildServiceIds).ToList();
    
    foreach (var childId in itemsToRemove)
    {
      service.RemoveServiceItem(childId);
    }

    // Agregar nuevos items
    var itemsToAdd = newChildServiceIds.Except(currentChildIds).ToList();
    
    for (int i = 0; i < itemsToAdd.Count; i++)
    {
      var displayOrder = service.ServiceItems.Count + i;
      service.AddServiceItem(itemsToAdd[i], quantity: 1, displayOrder: displayOrder);
    }

    return Result.Success();
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