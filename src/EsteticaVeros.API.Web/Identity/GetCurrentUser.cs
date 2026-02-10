using System.Security.Claims;
using EsteticaVeros.API.UseCases.Identity;
using EsteticaVeros.API.UseCases.Identity.GetCurrentUser;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Identity;

public class GetCurrentUser(IMediator mediator)
  : EndpointWithoutRequest<Results<Ok<CurrentUserResponse>,
                                  UnauthorizedHttpResult,
                                  NotFound>,
                                GetCurrentUserMapper>
{
  public override void Configure()
  {
    Get(GetCurrentUserRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);
    
    Summary(s =>
    {
      s.Summary = "Lista el perfil del usuario autenticado";
      s.Description = "Obtiene la información del perfil del usuario actualmente autenticado utilizando el token JWT proporcionado en la solicitud.";
      s.ResponseExamples[200] = new CurrentUserResponse(
        1,
        "admin@esteticaveros.com",
        "Admin Sistema",
        null,
        "Administrator"
      );

      s.Responses[200] = "Perfil del usuario recuperado exitosamente";
      s.Responses[401] = "No autenticado o token inválido";
      s.Responses[404] = "Usuario no encontrado";
    });

    Tags("Authentication");

    Description(builder => builder
      .Produces<CurrentUserResponse>(200, "application/json")
      .ProducesProblem(401)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<CurrentUserResponse>, UnauthorizedHttpResult, NotFound>>
    ExecuteAsync(CancellationToken ct)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User.FindFirst("userId")?.Value;

    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
    {
      return TypedResults.Unauthorized();
    }

    var query = new GetCurrentUserQuery(userId);
    var result = await mediator.Send(query, ct);

    if (!result.IsSuccess)
    {
      if (result.Status == ResultStatus.NotFound)
      {
        return TypedResults.NotFound();
      }
      return TypedResults.Unauthorized();
    }

    return TypedResults.Ok(Map.FromEntity(result.Value));
  }
}

public static class GetCurrentUserRequest
{
  public const string Route = "/api/auth/me";
}

public record CurrentUserResponse(
  int Id,
  string Email,
  string FullName,
  string? ProfilePictureUrl,
  string Role
);

public sealed class GetCurrentUserMapper
  : ResponseMapper<CurrentUserResponse, UserDTO>
{
  public override CurrentUserResponse FromEntity(UserDTO e)
    => new(
      e.Id,
      e.Email,
      e.FullName,
      e.ProfilePictureUrl,
      e.RoleName
    );
}