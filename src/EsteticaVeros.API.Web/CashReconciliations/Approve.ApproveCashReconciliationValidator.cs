using FluentValidation;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class ApproveCashReconciliationValidator : Validator<ApproveCashReconciliationRequest>
{
  public ApproveCashReconciliationValidator()
  {
    RuleFor(x => x.ReconciliationId)
      .GreaterThan(0);

    RuleFor(x => x.Notes)
      .MaximumLength(1000)
      .When(x => !string.IsNullOrWhiteSpace(x.Notes));
  }
}