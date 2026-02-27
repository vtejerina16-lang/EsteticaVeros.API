namespace EsteticaVeros.API.UseCases.ServiceCategories.Create;

public record CreateServiceCategoryCommand(
  string Name,
  string? Description = null,
  int DisplayOrder = 0,
  string? IconName = null,
  string? ColorCode = null,
  int? CreatedByUserId = null
) : ICommand<Result<ServiceCategoryDTO>>;