using FluentValidation;

namespace EsteticaVeros.API.Web.CashRegisters;

public class CreateCashRegisterValidator : Validator<CreateCashRegisterRequest>
{
  public CreateCashRegisterValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.Location)
      .MaximumLength(200)
      .When(x => !string.IsNullOrWhiteSpace(x.Location));
  }
}