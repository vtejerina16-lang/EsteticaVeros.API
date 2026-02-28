namespace EsteticaVeros.API.Web.Services;

public record ServicesListResponse(
  IReadOnlyList<ServiceRecord> Services
);