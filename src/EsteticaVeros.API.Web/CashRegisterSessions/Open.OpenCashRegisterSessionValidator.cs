using FluentValidation;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class OpenCashRegisterSessionValidator : Validator<OpenCashRegisterSessionRequest>
{
  public OpenCashRegisterSessionValidator()
  {
    RuleFor(x => x.CashRegisterId)
      .GreaterThan(0);

    RuleFor(x => x.InitialAmount)
      .GreaterThanOrEqualTo(0);
  }
}