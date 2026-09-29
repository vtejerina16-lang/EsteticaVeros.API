namespace EsteticaVeros.API.Core.ServiceAggregate.Specifications;

public sealed class SimpleServicesSpec : Specification<Service>
{
  public SimpleServicesSpec(bool activeOnly = true)
  {
    Query.Where(s => s.ServiceType == ServiceType.Simple);

    if (activeOnly)
    {
      Query.Where(s => s.IsActive);
    }

    Query
      .OrderBy(s => s.DisplayOrder)
      .ThenBy(s => s.Name);
  }
}