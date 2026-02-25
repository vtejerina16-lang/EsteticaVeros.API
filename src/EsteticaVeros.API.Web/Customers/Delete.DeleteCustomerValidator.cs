using FluentValidation;

namespace EsteticaVeros.API.Web.Customers;

public class DeleteCustomerValidator : Validator<DeleteCustomerRequest>
{
  public DeleteCustomerValidator()
  {
    RuleFor(x => x.CustomerId)
      .GreaterThan(0);
  }
}