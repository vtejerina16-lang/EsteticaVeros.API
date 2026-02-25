using EsteticaVeros.API.Web.Configurations;
using FluentValidation;

namespace EsteticaVeros.API.Web.Identity;

/// <summary>
/// Validator for updating a user.
/// Ensures all required fields are valid and route/body IDs match.
/// </summary>
public class UpdateUserValidator : Validator<UpdateUserRequest>
{
  public UpdateUserValidator()
  {
    RuleFor(x => x.FirstName)
      .NotEmpty()
      .WithMessage("First name is required")
      .MinimumLength(2)
      .WithMessage("First name must be at least 2 characters")
      .MaximumLength(50)
      .WithMessage("First name cannot exceed 50 characters");

    RuleFor(x => x.LastName)
      .NotEmpty()
      .WithMessage("Last name is required")
      .MinimumLength(2)
      .WithMessage("Last name must be at least 2 characters")
      .MaximumLength(50)
      .WithMessage("Last name cannot exceed 50 characters");

    RuleFor(x => x.PhoneNumber)
      .MaximumLength(20)
      .WithMessage("Phone number cannot exceed 20 characters")
      .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

    RuleFor(x => x.Address)
      .MaximumLength(200)
      .WithMessage("Address cannot exceed 200 characters")
      .When(x => !string.IsNullOrEmpty(x.Address));

    RuleFor(x => x.RoleName)
      .NotEmpty()
      .WithMessage("Role is required")
      .Must(BeAValidRole)
      .WithMessage($"Invalid role. Valid roles are: {string.Join(", ", GetValidRoles())}");

    RuleFor(x => x.UserId)
      .GreaterThan(0)
      .WithMessage("User ID must be greater than 0");
  }

  private static bool BeAValidRole(string roleName)
  {
    var validRoles = GetValidRoles();
    return validRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
  }

  private static string[] GetValidRoles()
  {
    return new[]
    {
      AuthorizationConfig.AdminRole,
      AuthorizationConfig.ManagerRole,
      AuthorizationConfig.AssistantRole
    };
  }
}