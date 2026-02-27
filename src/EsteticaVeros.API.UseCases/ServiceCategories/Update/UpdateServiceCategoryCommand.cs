namespace EsteticaVeros.API.UseCases.ServiceCategories.Update;

public record UpdateServiceCategoryCommand(
  int CategoryId,
  string Name,
  string? Description = null,
  int DisplayOrder = 0,
  string? IconName = null,
  string? ColorCode = null,
  bool IsActive = true
) : ICommand<Result<ServiceCategoryDTO>>;