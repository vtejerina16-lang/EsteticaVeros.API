namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Specifications;

public sealed class ServiceCategoryByIdSpec : Specification<ServiceCategory>, ISingleResultSpecification<ServiceCategory>
{
  public ServiceCategoryByIdSpec(int categoryId)
  {
    Query.Where(c => c.Id == categoryId);
  }
}