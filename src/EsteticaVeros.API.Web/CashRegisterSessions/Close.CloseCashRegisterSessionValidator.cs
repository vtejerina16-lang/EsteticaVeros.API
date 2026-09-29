using FluentValidation;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class CloseCashRegisterSessionValidator : Validator<CloseCashRegisterSessionRequest>
{
  public CloseCashRegisterSessionValidator()
  {
    RuleFor(x => x.SessionId)
      .GreaterThan(0);

    RuleFor(x => x.Notes)
      .MaximumLength(1000)
      .When(x => !string.IsNullOrWhiteSpace(x.Notes));
  }
}