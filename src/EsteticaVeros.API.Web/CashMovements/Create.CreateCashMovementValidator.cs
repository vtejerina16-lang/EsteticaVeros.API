using FluentValidation;

namespace EsteticaVeros.API.Web.CashMovements;

public class CreateCashMovementValidator : Validator<CreateCashMovementRequest>
{
  public CreateCashMovementValidator()
  {
    RuleFor(x => x.CashRegisterSessionId)
      .GreaterThan(0);

    RuleFor(x => x.Type)
      .InclusiveBetween(1, 6)
      .WithMessage("Type must be: 1=Income, 2=Expense, 3=Withdrawal, 4=Deposit, 5=Transfer, 6=Adjustment");

    RuleFor(x => x.Amount)
      .GreaterThan(0);

    RuleFor(x => x.Description)
      .NotEmpty()
      .MaximumLength(500);

    RuleFor(x => x.Reference)
      .MaximumLength(100)
      .When(x => !string.IsNullOrWhiteSpace(x.Reference));
  }
}