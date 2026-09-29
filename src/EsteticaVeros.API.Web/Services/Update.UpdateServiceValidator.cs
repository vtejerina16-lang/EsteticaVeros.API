using FluentValidation;

namespace EsteticaVeros.API.Web.Services;

public class UpdateServiceValidator : Validator<UpdateServiceRequest>
{
  public UpdateServiceValidator()
  {
    RuleFor(x => x.ServiceId)
      .GreaterThan(0);

    RuleFor(x => x.Name)
      .NotEmpty()
      .MinimumLength(2)
      .MaximumLength(200);

    RuleFor(x => x.Duration)
      .GreaterThan(0);

    RuleFor(x => x.BasePrice)
      .GreaterThanOrEqualTo(0);

    RuleFor(x => x.DepositAmount)
      .GreaterThan(0)
      .When(x => x.RequiresDeposit)
      .WithMessage("DepositAmount must be greater than 0 when RequiresDeposit is true");
  }
}