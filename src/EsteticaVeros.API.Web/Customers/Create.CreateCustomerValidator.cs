using FluentValidation;

namespace EsteticaVeros.API.Web.Customers;

public class CreateCustomerValidator : Validator<CreateCustomerRequest>
{
  public CreateCustomerValidator()
  {
    RuleFor(x => x.FirstName)
      .NotEmpty().MinimumLength(2).MaximumLength(50);

    RuleFor(x => x.LastName)
      .NotEmpty().MinimumLength(2).MaximumLength(50);

    RuleFor(x => x.PhoneNumber)
      .NotEmpty().MinimumLength(7).MaximumLength(20);

    RuleFor(x => x.Gender)
      .InclusiveBetween(1, 4)
      .WithMessage("Gender must be: 1=Male, 2=Female, 3=Other, 4=PreferNotToSay");

    RuleFor(x => x.Email)
      .EmailAddress().MaximumLength(100)
      .When(x => !string.IsNullOrWhiteSpace(x.Email));

    RuleFor(x => x.DateOfBirth)
      .LessThan(DateTime.Today)
      .When(x => x.DateOfBirth.HasValue);
  }
}