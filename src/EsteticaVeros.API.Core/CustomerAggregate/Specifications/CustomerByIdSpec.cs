namespace EsteticaVeros.API.Core.CustomerAggregate.Specifications;

public sealed class CustomerByIdSpec : Specification<Customer>, ISingleResultSpecification<Customer>
{
  public CustomerByIdSpec(int customerId)
  {
    Query.Where(c => c.Id == customerId);
  }
}