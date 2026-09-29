using EsteticaVeros.API.Web.Configurations;
using FluentValidation;

namespace EsteticaVeros.API.Web.Identity;

public class CreateUserValidator : Validator<CreateUserRequest>
{
  public CreateUserValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty()
      .WithMessage("Email is required")
      .EmailAddress()
      .WithMessage("Invalid email format")
      .MaximumLength(100);

    RuleFor(x => x.Password)
      .NotEmpty()
      .WithMessage("Password is required")
      .MinimumLength(8)
      .WithMessage("Password must be at least 8 characters")
      .Matches(@"[A-Z]")
      .WithMessage("Password must contain at least one uppercase letter")
      .Matches(@"[a-z]")
      .WithMessage("Password must contain at least one lowercase letter")
      .Matches(@"[0-9]")
      .WithMessage("Password must contain at least one digit")
      .Matches(@"[\W_]")
      .WithMessage("Password must contain at least one non-alphanumeric character");

    RuleFor(x => x.FirstName)
      .NotEmpty()
      .WithMessage("First name is required")
      .MinimumLength(2)
      .MaximumLength(50);

    RuleFor(x => x.LastName)
      .NotEmpty()
      .WithMessage("Last name is required")
      .MinimumLength(2)
      .MaximumLength(50);

    RuleFor(x => x.PhoneNumber)
      .MaximumLength(20)
      .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

    RuleFor(x => x.RoleName)
      .NotEmpty()
      .WithMessage("Role is required")
      .Must(BeAValidRole)
      .WithMessage($"Invalid role. Valid roles are: {string.Join(", ", GetValidRoles())}");
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