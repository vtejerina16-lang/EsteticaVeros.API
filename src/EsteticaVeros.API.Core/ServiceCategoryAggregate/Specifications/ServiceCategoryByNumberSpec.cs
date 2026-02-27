namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Specifications;

public sealed class ServiceCategoryByNumberSpec : Specification<ServiceCategory>, ISingleResultSpecification<ServiceCategory>
{
  public ServiceCategoryByNumberSpec(CategoryNumber categoryNumber)
  {
    Query.Where(c => c.CategoryNumber == categoryNumber);
  }
}