using Ardalis.Specification;
using EsteticaVeros.API.Core.ServiceAggregate;
using EsteticaVeros.API.Core.ServiceAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Services.List;

public class ListServicesHandler(
  IRepository<Service> repository,
  ILogger<ListServicesHandler> logger) 
  : IQueryHandler<ListServicesQuery, Result<List<ServiceDTO>>>
{
  public async ValueTask<Result<List<ServiceDTO>>> Handle(
    ListServicesQuery query,
    CancellationToken cancellationToken)
  {
    try
    {
      Specification<Service> spec = new Specification<Service>();

      // Filter by ServiceType
      if (query.ServiceType.HasValue)
      {
        var serviceType = ServiceType.FromValue(query.ServiceType.Value);
        spec.Query.Where(s => s.ServiceType == serviceType);
      }

      // Filter by CategoryId
      if (query.CategoryId.HasValue)
      {
        spec.Query.Where(s => s.CategoryId == query.CategoryId.Value);
      }

      // Filter by IsActive
      if (query.IsActive.HasValue)
      {
        spec.Query.Where(s => s.IsActive == query.IsActive.Value);
      }

      // Filter by IsFeatured
      if (query.IsFeatured.HasValue)
      {
        spec.Query.Where(s => s.IsFeatured == query.IsFeatured.Value);
      }

      // Include ServiceItems
      spec.Query.Include(s => s.ServiceItems);

      // Order by DisplayOrder and Name
      spec.Query
        .OrderBy(s => s.DisplayOrder)
        .ThenBy(s => s.Name);

      var services = await repository.ListAsync(spec, cancellationToken);
      
      var serviceDtos = new List<ServiceDTO>();
      
      foreach (var service in services)
      {
        serviceDtos.Add(await MapToDTO(service, repository, cancellationToken));
      }

      logger.LogInformation("Retrieved {Count} services", serviceDtos.Count);

      return Result.Success(serviceDtos);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error listing services");
      return Result.Error("Error retrieving services");
    }
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
      items,
      null // CategoryName es opcional ya que cruzamos con CategoryId
    );
  }
}