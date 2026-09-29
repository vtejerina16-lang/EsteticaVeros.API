using EsteticaVeros.API.Core.ServiceCategoryAggregate;

namespace EsteticaVeros.API.UseCases.ServiceCategories.Create;

public class CreateServiceCategoryHandler(
  IRepository<ServiceCategory> repository,
  ILogger<CreateServiceCategoryHandler> logger) 
  : ICommandHandler<CreateServiceCategoryCommand, Result<ServiceCategoryDTO>>
{
  public async ValueTask<Result<ServiceCategoryDTO>> Handle(
    CreateServiceCategoryCommand command,
    CancellationToken cancellationToken)
  {
    // Generate category number
    var sequence = await GetNextSequenceNumber(cancellationToken);
    var categoryNumber = CategoryNumber.Generate(sequence);

    // Create category
    var category = new ServiceCategory(
      categoryNumber,
      command.Name,
      command.Description,
      command.DisplayOrder,
      command.CreatedByUserId
    );

    // Update visual settings if provided
    if (!string.IsNullOrWhiteSpace(command.IconName) || !string.IsNullOrWhiteSpace(command.ColorCode))
    {
      category.UpdateVisualSettings(command.IconName, command.ColorCode);
    }

    var createdCategory = await repository.AddAsync(category, cancellationToken);

    logger.LogInformation(
      "Service category created: {CategoryNumber} - {Name}",
      createdCategory.CategoryNumber,
      createdCategory.Name);

    return Result.Success(MapToDTO(createdCategory));
  }

  private async Task<int> GetNextSequenceNumber(CancellationToken cancellationToken)
  {
    var categories = await repository.ListAsync(cancellationToken);
    
    var highestSequence = categories
      .Select(c =>
      {
        var parts = c.CategoryNumber.Value.Split('-');
        return parts.Length == 2 && int.TryParse(parts[1], out var seq) ? seq : 0;
      })
      .DefaultIfEmpty(0)
      .Max();

    return highestSequence + 1;
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