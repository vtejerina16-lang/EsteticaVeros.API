namespace EsteticaVeros.API.Core.ServiceAggregate.Specifications;

public sealed class ServicesByTypeSpec : Specification<Service>
{
  public ServicesByTypeSpec(ServiceType serviceType, bool activeOnly = true)
  {
    Query.Where(s => s.ServiceType == serviceType);

    if (activeOnly)
    {
      Query.Where(s => s.IsActive);
    }

    Query
      .OrderBy(s => s.DisplayOrder)
      .ThenBy(s => s.Name);
  }
}