using FluentValidation;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class RejectCashReconciliationValidator : Validator<RejectCashReconciliationRequest>
{
  public RejectCashReconciliationValidator()
  {
    RuleFor(x => x.ReconciliationId)
      .GreaterThan(0);

    RuleFor(x => x.Reason)
      .NotEmpty()
      .MaximumLength(1000);
  }
}