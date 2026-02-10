using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.UseCases.Identity.CreateUser;

public class CreateUserHandler(
  UserManager<ApplicationUser> userManager,
  RoleManager<ApplicationRole> roleManager,
  ILogger<CreateUserHandler> logger) : ICommandHandler<CreateUserCommand, Result<UserDTO>>
{
  public async ValueTask<Result<UserDTO>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
  {
    // Check if user already exists
    var existingUser = await userManager.FindByEmailAsync(command.Email);
    if (existingUser != null)
    {
      return Result.Error("A user with this email already exists.");
    }

    // Validate that role exists
    var roleExists = await roleManager.RoleExistsAsync(command.RoleName);
    if (!roleExists)
    {
      logger.LogWarning("Attempted to create user with non-existent role: {RoleName}", command.RoleName);
      return Result.Error($"Role '{command.RoleName}' does not exist.");
    }

    // Create new user
    var user = new ApplicationUser
    {
      UserName = command.Email,
      Email = command.Email,
      FirstName = command.FirstName,
      LastName = command.LastName,
      PhoneNumber = command.PhoneNumber,
      EmailConfirmed = true, // Auto-confirm for internal system
      IsActive = true
    };

    // Create user with password
    var result = await userManager.CreateAsync(user, command.Password);

    if (!result.Succeeded)
    {
      var errors = string.Join(", ", result.Errors.Select(e => e.Description));
      logger.LogWarning("Failed to create user {Email}: {Errors}", command.Email, errors);
      return Result.Error(errors);
    }

    // Assign default role (Receptionist or whatever is appropriate)
    var roleResult = await userManager.AddToRoleAsync(user, command.RoleName);
    if (!roleResult.Succeeded)
    {
      var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
      logger.LogWarning("Failed to assign role {RoleName} to user {Email}: {Errors}", command.RoleName, command.Email, errors);

      // Rollback: delete the user if role assignment fails
      await userManager.DeleteAsync(user);
      return Result.Error($"Failed to assign role: {errors}");
    }

    logger.LogInformation("User created successfully: {Email} with role {RoleName}", command.Email, command.RoleName);

    // Return user DTO
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
      command.RoleName
    );

    return Result.Success(userDto);
  }
}