namespace EsteticaVeros.API.Web.Services;

public record ServiceRecord(
  int Id,
  string ServiceNumber,
  string Name,
  string ServiceType,
  int Duration,
  decimal BasePrice,
  bool IsActive
);