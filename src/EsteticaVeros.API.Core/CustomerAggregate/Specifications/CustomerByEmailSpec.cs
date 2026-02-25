namespace EsteticaVeros.API.Core.CustomerAggregate.Specifications;

public sealed class CustomerByEmailSpec : Specification<Customer>, ISingleResultSpecification<Customer>
{
  public CustomerByEmailSpec(string email)
  {
    var normalizedEmail = email.ToUpperInvariant();
    Query.Where(c => c.Email != null && c.Email.ToUpper() == normalizedEmail);
  }
}