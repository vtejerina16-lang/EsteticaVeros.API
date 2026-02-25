namespace EsteticaVeros.API.Core.CustomerAggregate.Specifications;

public sealed class ActiveCustomersSpec : Specification<Customer>
{
  public ActiveCustomersSpec()
  {
    Query
      .Where(c => c.IsActive)
      .OrderBy(c => c.FirstName)
      .ThenBy(c => c.LastName);
  }
}