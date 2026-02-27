namespace EsteticaVeros.API.UseCases.ServiceCategories.List;

public record ListServiceCategoriesQuery(
  bool? IsActive = null
) : IQuery<Result<List<ServiceCategoryDTO>>>;