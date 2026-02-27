namespace EsteticaVeros.API.UseCases.ServiceCategories.Get;

public record GetServiceCategoryQuery(int CategoryId) : IQuery<Result<ServiceCategoryDTO>>;