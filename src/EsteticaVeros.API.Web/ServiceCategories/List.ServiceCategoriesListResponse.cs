namespace EsteticaVeros.API.Web.ServiceCategories;

public record ServiceCategoriesListResponse(
  IReadOnlyList<ServiceCategoryRecord> Categories
);