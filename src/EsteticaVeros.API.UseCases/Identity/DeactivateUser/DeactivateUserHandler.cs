using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.UseCases.Identity.DeactivateUser;

public class DeactivateUserHandler(
  UserManager<ApplicationUser> userManager,
  ILogger<DeactivateUserHandler> logger) : ICommandHandler<DeactivateUserCommand, Result>
{
  public async ValueTask<Result> Handle(
    DeactivateUserCommand command,
    CancellationToken cancellationToken)
  {
    var user = await userManager.FindByIdAsync(command.UserId.ToString());
    if (user == null)
    {
      return Result.NotFound();
    }

    // Don't allow deactivating the main admin
    if (user.Email == "admin@esteticaveros.com")
    {
      return Result.Error("Cannot deactivate the main admin account.");
    }

    user.Deactivate();
    var result = await userManager.UpdateAsync(user);

    if (result.Succeeded)
    {
      logger.LogInformation("User {UserId} deactivated", command.UserId);
      return Result.Success();
    }

    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
    return Result.Error(errors);
  }
}