namespace EsteticaVeros.API.Core.CustomerAggregate.Specifications;

public sealed class CustomerByPhoneNumberSpec : Specification<Customer>, ISingleResultSpecification<Customer>
{
  public CustomerByPhoneNumberSpec(string phoneNumber)
  {
    var normalized = new string(phoneNumber.Where(char.IsDigit).ToArray());
    Query.Where(c => c.PhoneNumber.Contains(normalized));
  }
}