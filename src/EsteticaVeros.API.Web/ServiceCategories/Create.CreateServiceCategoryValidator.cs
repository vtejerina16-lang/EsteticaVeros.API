using FluentValidation;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class CreateServiceCategoryValidator : Validator<CreateServiceCategoryRequest>
{
  public CreateServiceCategoryValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MinimumLength(2)
      .MaximumLength(100);

    RuleFor(x => x.Description)
      .MaximumLength(500)
      .When(x => !string.IsNullOrWhiteSpace(x.Description));

    RuleFor(x => x.DisplayOrder)
      .GreaterThanOrEqualTo(0);

    RuleFor(x => x.IconName)
      .MaximumLength(50)
      .When(x => !string.IsNullOrWhiteSpace(x.IconName));

    RuleFor(x => x.ColorCode)
      .MaximumLength(20)
      .When(x => !string.IsNullOrWhiteSpace(x.ColorCode));
  }
}