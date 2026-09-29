using System.Security.Claims;

namespace EsteticaVeros.API.Web.Extensions;

public static class UserExtensions
{
  public static int? GetUserId(this ClaimsPrincipal user)
  {
    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? user.FindFirst("userId")?.Value;

    return int.TryParse(userIdClaim, out var userId) ? userId : null;
  }
}