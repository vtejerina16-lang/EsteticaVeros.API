namespace EsteticaVeros.API.Web.Services;

public class ListServicesRequest
{
  [BindFrom("service_type")]
  public int? ServiceType { get; init; }

  [BindFrom("category_id")]
  public int? CategoryId { get; init; }

  [BindFrom("is_active")]
  public bool? IsActive { get; init; }

  [BindFrom("is_featured")]
  public bool? IsFeatured { get; init; }
}