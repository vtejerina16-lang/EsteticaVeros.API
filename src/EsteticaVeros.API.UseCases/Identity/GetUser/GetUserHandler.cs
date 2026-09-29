using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.UseCases.Identity.GetUser;

public class GetUserHandler(
  UserManager<ApplicationUser> userManager,
  ILogger<GetUserHandler> logger) : IQueryHandler<GetUserQuery, Result<UserDTO>>
{
  public async ValueTask<Result<UserDTO>> Handle(
    GetUserQuery query, 
    CancellationToken cancellationToken)
  {
    var user = await userManager.FindByIdAsync(query.UserId.ToString());

    if (user == null)
    {
      logger.LogWarning("User not found: {UserId}", query.UserId);
      return Result.NotFound();
    }

    var roles = await userManager.GetRolesAsync(user);
    var roleName = roles.FirstOrDefault() ?? "None";

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