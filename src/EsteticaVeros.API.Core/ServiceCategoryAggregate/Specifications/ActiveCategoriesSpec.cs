namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Specifications;

public sealed class ActiveCategoriesSpec : Specification<ServiceCategory>
{
  public ActiveCategoriesSpec()
  {
    Query
      .Where(c => c.IsActive)
      .OrderBy(c => c.DisplayOrder)
      .ThenBy(c => c.Name);
  }
}