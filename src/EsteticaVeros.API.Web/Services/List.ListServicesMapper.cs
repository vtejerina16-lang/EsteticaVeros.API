using EsteticaVeros.API.UseCases.Services;

namespace EsteticaVeros.API.Web.Services;

public sealed class ListServicesMapper
  : Mapper<ListServicesRequest, ServicesListResponse, List<ServiceDTO>>
{
  public override ServicesListResponse FromEntity(List<ServiceDTO> e)
  {
    var services = e
      .Select(s => new ServiceRecord(
        s.Id,
        s.ServiceNumber,
        s.Name,
        s.ServiceTypeName,
        s.Duration,
        s.BasePrice,
        s.IsActive,
        s.CategoryId,
        s.CategoryName
      ))
      .ToList();

    return new ServicesListResponse(services);
  }
}