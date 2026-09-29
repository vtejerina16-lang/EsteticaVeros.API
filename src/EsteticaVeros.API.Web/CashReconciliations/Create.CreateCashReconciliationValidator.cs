using FluentValidation;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class CreateCashReconciliationValidator : Validator<CreateCashReconciliationRequest>
{
  public CreateCashReconciliationValidator()
  {
    RuleFor(x => x.CashRegisterSessionId)
      .GreaterThan(0);

    RuleFor(x => x.CountedAmount)
      .GreaterThanOrEqualTo(0);

    RuleFor(x => x.DenominationDetails)
      .MaximumLength(2000)
      .When(x => !string.IsNullOrWhiteSpace(x.DenominationDetails));

    RuleFor(x => x.Notes)
      .MaximumLength(1000)
      .When(x => !string.IsNullOrWhiteSpace(x.Notes));
  }
}