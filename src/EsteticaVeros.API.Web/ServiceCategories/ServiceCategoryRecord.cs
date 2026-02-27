namespace EsteticaVeros.API.Web.ServiceCategories;

public record ServiceCategoryRecord(
  int Id,
  string CategoryNumber,
  string Name,
  int DisplayOrder,
  string? IconName,
  string? ColorCode,
  bool IsActive
);