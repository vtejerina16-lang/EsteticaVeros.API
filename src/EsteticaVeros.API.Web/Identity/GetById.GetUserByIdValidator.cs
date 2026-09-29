using FastEndpoints;
using FluentValidation;

namespace EsteticaVeros.API.Web.Identity;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetUserByIdValidator : Validator<GetUserByIdRequest>
{
  public GetUserByIdValidator()
  {
    RuleFor(x => x.UserId)
      .GreaterThan(0);
  }
}
