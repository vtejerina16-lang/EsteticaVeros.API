namespace EsteticaVeros.API.UseCases.ServiceCategories;

public record ServiceCategoryDTO(
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