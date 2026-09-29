namespace EsteticaVeros.API.Web.ServiceCategories;

public record ServiceCategoryDetailResponse(
  int Id,
  string CategoryNumber,
  string Name,
  string? Description,
  int DisplayOrder,
  string? IconName,
  string? ColorCode,
  bool IsActive,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);