namespace EsteticaVeros.API.UseCases.Services.List;

public record ListServicesQuery(
  int? ServiceType = null,
  int? CategoryId = null,
  bool? IsActive = null,
  bool? IsFeatured = null
) : IQuery<Result<List<ServiceDTO>>>;