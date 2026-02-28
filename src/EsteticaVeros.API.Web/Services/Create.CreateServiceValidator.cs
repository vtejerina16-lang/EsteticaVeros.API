using FluentValidation;

namespace EsteticaVeros.API.Web.Services;

public class CreateServiceValidator : Validator<CreateServiceRequest>
{
  public CreateServiceValidator()
  {
    RuleFor(x => x.ServiceType)
      .InclusiveBetween(1, 3)
      .WithMessage("ServiceType must be: 1=Simple, 2=Composite, 3=Package");

    RuleFor(x => x.Name)
      .NotEmpty()
      .MinimumLength(2)
      .MaximumLength(200);

    RuleFor(x => x.Duration)
      .GreaterThan(0)
      .WithMessage("Duration must be greater than 0 minutes");

    RuleFor(x => x.BasePrice)
      .GreaterThanOrEqualTo(0)
      .WithMessage("BasePrice must be 0 or greater");

    RuleFor(x => x.CategoryId)
      .NotNull()
      .When(x => x.ServiceType == 1 || x.ServiceType == 2)
      .WithMessage("CategoryId is required for Simple and Composite services");

    RuleFor(x => x.CategoryId)
      .Null()
      .When(x => x.ServiceType == 3)
      .WithMessage("Packages cannot have a single CategoryId");

    RuleFor(x => x.ChildServiceIds)
      .NotEmpty()
      .When(x => x.ServiceType == 2 || x.ServiceType == 3)
      .WithMessage("Composite and Package services must have child services");

    RuleFor(x => x.ChildServiceIds)
      .Must(ids => ids == null || ids.Count >= 2)
      .When(x => x.ServiceType == 2 || x.ServiceType == 3)
      .WithMessage("Composite and Package services must have at least 2 child services");

    RuleFor(x => x.ChildServiceIds)
      .Must(ids => ids == null || ids.Count <= 10)
      .When(x => x.ServiceType == 2 || x.ServiceType == 3)
      .WithMessage("Composite and Package services cannot have more than 10 child services");

    RuleFor(x => x.DepositAmount)
      .GreaterThan(0)
      .When(x => x.RequiresDeposit)
      .WithMessage("DepositAmount must be greater than 0 when RequiresDeposit is true");
  }
}