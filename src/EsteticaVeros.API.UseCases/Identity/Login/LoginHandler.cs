using EsteticaVeros.API.Core.IdentityAggregate;
using EsteticaVeros.API.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.UseCases.Identity.Login;

public class LoginHandler(
  UserManager<ApplicationUser> userManager,
  SignInManager<ApplicationUser> signInManager,
  IJwtTokenGenerator jwtTokenGenerator,
  ILogger<LoginHandler> logger) : ICommandHandler<LoginCommand, Result<LoginResult>>
{
  public async ValueTask<Result<LoginResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
  {
    // Find user by email
    var user = await userManager.FindByEmailAsync(command.Email);
    if (user == null)
    {
      logger.LogWarning("Login attempt with non-existent email: {Email}", command.Email);
      return Result.Unauthorized();
    }

    // Check if user is active
    if (!user.IsActive)
    {
      logger.LogWarning("Login attempt for inactive user: {Email}", command.Email);
      return Result.Forbidden();
    }

    // Attempt sign in
    var result = await signInManager.PasswordSignInAsync(
      user,
      command.Password,
      command.RememberMe,
      lockoutOnFailure: true
    );

    if (result.Succeeded)
    {
      // Update last login
      user.RecordLogin();
      await userManager.UpdateAsync(user);

      logger.LogInformation("User logged in: {Email}", command.Email);

      // Get user role
      var roles = await userManager.GetRolesAsync(user);
      var roleName = roles.FirstOrDefault() ?? "User";

      // Create user DTO
      var userDto = new UserDTO(
        user.Id,
        user.Email!,
        user.FirstName,
        user.LastName,
        user.FullName,
        user.PhoneNumber,
        user.Address,
        user.DateOfBirth,
        user.ProfilePictureUrl,
        user.IsActive,
        user.CreatedAt,
        user.LastLoginAt,
        roleName
      );

      // Generate JWT token
      var token = jwtTokenGenerator.GenerateToken(user, roleName);
      var expiresAt = DateTime.UtcNow.AddHours(8);

      var loginResult = new LoginResult(userDto, token, expiresAt);
      return Result.Success(loginResult);
    }

    if (result.IsLockedOut)
    {
      logger.LogWarning("User account locked out: {Email}", command.Email);
      return Result.Error("Account is locked due to multiple failed login attempts.");
    }

    if (result.RequiresTwoFactor)
    {
      logger.LogInformation("Two-factor authentication required: {Email}", command.Email);
      return Result.Error("Two-factor authentication required.");
    }

    logger.LogWarning("Failed login attempt: {Email}", command.Email);
    return Result.Unauthorized();
  }
}