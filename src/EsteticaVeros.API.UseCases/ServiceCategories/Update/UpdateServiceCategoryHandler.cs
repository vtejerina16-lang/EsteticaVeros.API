using EsteticaVeros.API.Core.ServiceCategoryAggregate;
using EsteticaVeros.API.Core.ServiceCategoryAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.ServiceCategories.Update;

public class UpdateServiceCategoryHandler(
  IRepository<ServiceCategory> repository,
  ILogger<UpdateServiceCategoryHandler> logger) 
  : ICommandHandler<UpdateServiceCategoryCommand, Result<ServiceCategoryDTO>>
{
  public async ValueTask<Result<ServiceCategoryDTO>> Handle(
    UpdateServiceCategoryCommand command,
    CancellationToken cancellationToken)
  {
    var spec = new ServiceCategoryByIdSpec(command.CategoryId);
    var category = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (category == null)
    {
      return Result.NotFound();
    }

    // Update basic info
    category.UpdateBasicInfo(command.Name, command.Description);

    // Update display order
    category.UpdateDisplayOrder(command.DisplayOrder);

    // Update visual settings
    category.UpdateVisualSettings(command.IconName, command.ColorCode);

    // Update active status
    if (command.IsActive && !category.IsActive)
    {
      category.Activate();
    }
    else if (!command.IsActive && category.IsActive)
    {
      category.Deactivate();
    }

    await repository.UpdateAsync(category, cancellationToken);

    logger.LogInformation(
      "Service category updated: {CategoryNumber} - {Name}",
      category.CategoryNumber,
      category.Name);

    return Result.Success(MapToDTO(category));
  }

  private static ServiceCategoryDTO MapToDTO(ServiceCategory category)
  {
    return new ServiceCategoryDTO(
      category.Id,
      category.CategoryNumber.Value,
      category.Name,
      category.Description,
      category.DisplayOrder,
      category.IconName,
      category.ColorCode,
      category.IsActive,
      category.CreatedAt,
      category.UpdatedAt
    );
  }
}