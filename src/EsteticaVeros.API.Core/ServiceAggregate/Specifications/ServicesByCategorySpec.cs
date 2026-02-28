namespace EsteticaVeros.API.Core.ServiceAggregate.Specifications;

public sealed class ServicesByCategorySpec : Specification<Service>
{
  public ServicesByCategorySpec(int categoryId, bool activeOnly = true)
  {
    Query.Where(s => s.CategoryId == categoryId);

    if (activeOnly)
    {
      Query.Where(s => s.IsActive);
    }

    Query
      .OrderBy(s => s.DisplayOrder)
      .ThenBy(s => s.Name);
  }
}