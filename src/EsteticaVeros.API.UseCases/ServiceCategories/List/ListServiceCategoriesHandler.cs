using Ardalis.Specification;
using EsteticaVeros.API.Core.ServiceCategoryAggregate;
using EsteticaVeros.API.Core.ServiceCategoryAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.ServiceCategories.List;

public class ListServiceCategoriesHandler(
  IRepository<ServiceCategory> repository,
  ILogger<ListServiceCategoriesHandler> logger) 
  : IQueryHandler<ListServiceCategoriesQuery, Result<List<ServiceCategoryDTO>>>
{
  public async ValueTask<Result<List<ServiceCategoryDTO>>> Handle(
    ListServiceCategoriesQuery query,
    CancellationToken cancellationToken)
  {
    try
    {
      Specification<ServiceCategory> spec;

      if (query.IsActive.HasValue && query.IsActive.Value)
      {
        spec = new ActiveCategoriesSpec();
      }
      else if (query.IsActive.HasValue && !query.IsActive.Value)
      {
        spec = new Specification<ServiceCategory>();
        spec.Query
          .Where(c => !c.IsActive)
          .OrderBy(c => c.DisplayOrder)
          .ThenBy(c => c.Name);
      }
      else
      {
        spec = new Specification<ServiceCategory>();
        spec.Query
          .OrderBy(c => c.DisplayOrder)
          .ThenBy(c => c.Name);
      }

      var categories = await repository.ListAsync(spec, cancellationToken);
      var categoryDtos = categories.Select(MapToDTO).ToList();

      logger.LogInformation("Retrieved {Count} service categories", categoryDtos.Count);

      return Result.Success(categoryDtos);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error listing service categories");
      return Result.Error("Error retrieving service categories");
    }
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