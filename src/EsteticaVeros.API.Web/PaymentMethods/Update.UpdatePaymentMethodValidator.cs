using FluentValidation;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class UpdatePaymentMethodValidator : Validator<UpdatePaymentMethodRequest>
{
  public UpdatePaymentMethodValidator()
  {
    RuleFor(x => x.PaymentMethodId)
      .GreaterThan(0);

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.Type)
      .InclusiveBetween(1, 6)
      .WithMessage("Type must be: 1=Cash, 2=Card, 3=Transfer, 4=QR, 5=Check, 6=Other");
  }
}