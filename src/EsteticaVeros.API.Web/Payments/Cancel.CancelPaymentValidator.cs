using FluentValidation;

namespace EsteticaVeros.API.Web.Payments;

public class CancelPaymentValidator : Validator<CancelPaymentRequest>
{
  public CancelPaymentValidator()
  {
    RuleFor(x => x.PaymentId)
      .GreaterThan(0);

    RuleFor(x => x.Reason)
      .MaximumLength(500)
      .When(x => !string.IsNullOrWhiteSpace(x.Reason));
  }
}