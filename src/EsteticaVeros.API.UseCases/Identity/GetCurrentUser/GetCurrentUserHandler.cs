using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.AspNetCore.Identity;

namespace EsteticaVeros.API.UseCases.Identity.GetCurrentUser;

public class GetCurrentUserHandler(
  UserManager<ApplicationUser> userManager,
  ILogger<GetCurrentUserHandler> logger) : IQueryHandler<GetCurrentUserQuery, Result<UserDTO>>
{
  public async ValueTask<Result<UserDTO>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
  {
    var user = await userManager.FindByIdAsync(query.UserId.ToString());
    
    if (user == null)
    {
      logger.LogWarning("User not found: {UserId}", query.UserId);
      return Result.NotFound();
    }

    if (!user.IsActive)
    {
      logger.LogWarning("Inactive user attempted to get profile: {UserId}", query.UserId);
      return Result.Forbidden();
    }

    // Get user role
    var roles = await userManager.GetRolesAsync(user);
    var roleName = roles.FirstOrDefault() ?? "User";

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

    return Result.Success(userDto);
  }
}