using EsteticaVeros.API.Core.ServiceCategoryAggregate;
using EsteticaVeros.API.Core.ServiceCategoryAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.ServiceCategories.Get;

public class GetServiceCategoryHandler(IRepository<ServiceCategory> repository)
  : IQueryHandler<GetServiceCategoryQuery, Result<ServiceCategoryDTO>>
{
  public async ValueTask<Result<ServiceCategoryDTO>> Handle(
    GetServiceCategoryQuery query,
    CancellationToken cancellationToken)
  {
    var spec = new ServiceCategoryByIdSpec(query.CategoryId);
    var category = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (category == null)
    {
      return Result.NotFound();
    }

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