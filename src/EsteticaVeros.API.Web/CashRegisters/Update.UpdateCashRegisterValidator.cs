using FluentValidation;

namespace EsteticaVeros.API.Web.CashRegisters;

public class UpdateCashRegisterValidator : Validator<UpdateCashRegisterRequest>
{
  public UpdateCashRegisterValidator()
  {
    RuleFor(x => x.CashRegisterId)
      .GreaterThan(0);

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.Location)
      .MaximumLength(200)
      .When(x => !string.IsNullOrWhiteSpace(x.Location));
  }
}