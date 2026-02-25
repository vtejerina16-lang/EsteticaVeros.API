using FluentValidation;

namespace EsteticaVeros.API.Web.Customers;

public class UpdateCustomerValidator : Validator<UpdateCustomerRequest>
{
  public UpdateCustomerValidator()
  {
    RuleFor(x => x.CustomerId)
      .GreaterThan(0);

    RuleFor(x => x.FirstName)
      .NotEmpty().MinimumLength(2).MaximumLength(50);

    RuleFor(x => x.LastName)
      .NotEmpty().MinimumLength(2).MaximumLength(50);

    RuleFor(x => x.PhoneNumber)
      .NotEmpty().MinimumLength(7).MaximumLength(20);

    RuleFor(x => x.Gender)
      .InclusiveBetween(1, 4);

    RuleFor(x => x.Email)
      .EmailAddress().MaximumLength(100)
      .When(x => !string.IsNullOrWhiteSpace(x.Email));

    RuleFor(x => x.DateOfBirth)
      .LessThan(DateTime.Today)
      .When(x => x.DateOfBirth.HasValue);
  }
}