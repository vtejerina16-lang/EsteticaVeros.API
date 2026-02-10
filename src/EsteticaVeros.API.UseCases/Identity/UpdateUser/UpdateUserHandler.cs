using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.UseCases.Identity.UpdateUser;

public class UpdateUserHandler(
  UserManager<ApplicationUser> userManager,
  RoleManager<ApplicationRole> roleManager,
  ILogger<UpdateUserHandler> logger) : ICommandHandler<UpdateUserCommand, Result<UserDTO>>
{
  public async ValueTask<Result<UserDTO>> Handle(
    UpdateUserCommand command,
    CancellationToken cancellationToken)
  {
    var user = await userManager.FindByIdAsync(command.UserId.ToString());
    if (user == null)
    {
      return Result.NotFound();
    }

    // Validate role exists
    if (!await roleManager.RoleExistsAsync(command.RoleName))
    {
      return Result.Error($"Role '{command.RoleName}' does not exist.");
    }

    // Update user properties
    user.UpdateProfile(command.FirstName, command.LastName, command.Address, command.DateOfBirth);
    user.PhoneNumber = command.PhoneNumber;

    if (command.IsActive)
      user.Activate();
    else
      user.Deactivate();

    var updateResult = await userManager.UpdateAsync(user);
    if (!updateResult.Succeeded)
    {
      var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
      return Result.Error(errors);
    }

    // Update role if changed
    var currentRoles = await userManager.GetRolesAsync(user);
    var currentRole = currentRoles.FirstOrDefault();

    if (currentRole != command.RoleName)
    {
      if (currentRole != null)
      {
        await userManager.RemoveFromRoleAsync(user, currentRole);
      }
      await userManager.AddToRoleAsync(user, command.RoleName);
    }

    logger.LogInformation("User {UserId} updated successfully", command.UserId);

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