using FluentValidation;

namespace EsteticaVeros.API.Web.Payments;

public class CreatePaymentValidator : Validator<CreatePaymentRequest>
{
  public CreatePaymentValidator()
  {
    RuleFor(x => x.CustomerId)
      .GreaterThan(0);

    RuleFor(x => x.PaymentMethodId)
      .GreaterThan(0);

    RuleFor(x => x.Amount)
      .GreaterThan(0);

    RuleFor(x => x.AppointmentId)
      .GreaterThan(0)
      .When(x => x.AppointmentId.HasValue);

    RuleFor(x => x.CashRegisterSessionId)
      .GreaterThan(0)
      .When(x => x.CashRegisterSessionId.HasValue);

    RuleFor(x => x.Reference)
      .MaximumLength(100)
      .When(x => !string.IsNullOrWhiteSpace(x.Reference));

    RuleFor(x => x.Notes)
      .MaximumLength(500)
      .When(x => !string.IsNullOrWhiteSpace(x.Notes));
  }
}