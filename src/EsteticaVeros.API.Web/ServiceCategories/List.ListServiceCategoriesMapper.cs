using EsteticaVeros.API.UseCases.ServiceCategories;

namespace EsteticaVeros.API.Web.ServiceCategories;

public sealed class ListServiceCategoriesMapper
  : Mapper<ListServiceCategoriesRequest, ServiceCategoriesListResponse, List<ServiceCategoryDTO>>
{
  public override ServiceCategoriesListResponse FromEntity(List<ServiceCategoryDTO> e)
  {
    var categories = e
      .Select(c => new ServiceCategoryRecord(
        c.Id,
        c.CategoryNumber,
        c.Name,
        c.DisplayOrder,
        c.IconName,
        c.ColorCode,
        c.IsActive
      ))
      .ToList();

    return new ServiceCategoriesListResponse(categories);
  }
}