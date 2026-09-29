using FluentValidation;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class DeleteServiceCategoryValidator : Validator<DeleteServiceCategoryRequest>
{
  public DeleteServiceCategoryValidator()
  {
    RuleFor(x => x.CategoryId)
      .GreaterThan(0);
  }
}