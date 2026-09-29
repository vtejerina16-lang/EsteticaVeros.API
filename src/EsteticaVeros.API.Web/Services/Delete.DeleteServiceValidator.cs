using FluentValidation;

namespace EsteticaVeros.API.Web.Services;

public class DeleteServiceValidator : Validator<DeleteServiceRequest>
{
  public DeleteServiceValidator()
  {
    RuleFor(x => x.ServiceId)
      .GreaterThan(0);
  }
}