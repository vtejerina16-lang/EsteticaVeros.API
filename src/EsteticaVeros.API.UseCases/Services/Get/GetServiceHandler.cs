using EsteticaVeros.API.Core.ServiceAggregate;
using EsteticaVeros.API.Core.ServiceAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Services.Get;

public class GetServiceHandler(IRepository<Service> repository)
  : IQueryHandler<GetServiceQuery, Result<ServiceDTO>>
{
  public async ValueTask<Result<ServiceDTO>> Handle(
    GetServiceQuery query,
    CancellationToken cancellationToken)
  {
    var spec = new ServiceByIdSpec(query.ServiceId, includeItems: true);
    var service = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (service == null)
    {
      return Result.NotFound();
    }

    return Result.Success(await MapToDTO(service, repository, cancellationToken));
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