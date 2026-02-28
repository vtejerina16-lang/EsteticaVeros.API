namespace EsteticaVeros.API.Core.ServiceAggregate.Specifications;

public sealed class ServiceByNumberSpec : Specification<Service>, ISingleResultSpecification<Service>
{
  public ServiceByNumberSpec(ServiceNumber serviceNumber, bool includeItems = false)
  {
    Query.Where(s => s.ServiceNumber == serviceNumber);

    if (includeItems)
    {
      Query.Include(s => s.ServiceItems);
    }
  }
}