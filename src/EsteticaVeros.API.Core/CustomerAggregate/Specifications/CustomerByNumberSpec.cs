namespace EsteticaVeros.API.Core.CustomerAggregate.Specifications;

public sealed class CustomerByNumberSpec : Specification<Customer>, ISingleResultSpecification<Customer>
{
  public CustomerByNumberSpec(CustomerNumber customerNumber)
  {
    Query.Where(c => c.CustomerNumber == customerNumber);
  }
}