namespace EsteticaVeros.API.Core.ServiceAggregate.Specifications;

public sealed class ServiceByIdSpec : Specification<Service>, ISingleResultSpecification<Service>
{
  public ServiceByIdSpec(int serviceId, bool includeItems = false)
  {
    Query.Where(s => s.Id == serviceId);

    if (includeItems)
    {
      Query.Include(s => s.ServiceItems);
    }
  }
}