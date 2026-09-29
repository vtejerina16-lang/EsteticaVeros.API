using FluentValidation;

namespace EsteticaVeros.API.Web.Identity;

/// <summary>
/// Validator for deactivating a user request.
/// Ensures the user ID is valid.
/// </summary>
public class DeactivateUserValidator : Validator<DeactivateUserRequest>
{
  public DeactivateUserValidator()
  {
    RuleFor(x => x.UserId)
      .GreaterThan(0)
      .WithMessage("User ID must be greater than 0");
  }
}