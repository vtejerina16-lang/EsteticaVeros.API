using EsteticaVeros.API.UseCases.Identity.DeactivateUser;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Identity;

public class DeactivateUser(IMediator mediator)
  : Endpoint<DeactivateUserRequest,
            Results<NoContent,
              NotFound,
              ProblemHttpResult>>
{
  public override void Configure()
  {
    Delete(DeactivateUserRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Eliminar un usuario";
      s.Description = "Desactiva una cuenta de usuario (soft delete). El administrador principal no puede ser desactivado.";
      s.ExampleRequest = new DeactivateUserRequest { UserId = 2 };

      s.Responses[204] = "usuario desactivado exitosamente";
      s.Responses[400] = "No se puede desactivar el administrador principal o falló la desactivación";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administradores)";
      s.Responses[404] = "Usuario no encontrado";
    });

    Tags("Users");

    Description(builder => builder
      .Accepts<DeactivateUserRequest>()
      .Produces(204)
      .ProducesProblem(400)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>> ExecuteAsync(
    DeactivateUserRequest req,
    CancellationToken ct)
  {
    var command = new DeactivateUserCommand(req.UserId);
    var result = await mediator.Send(command, ct);

    if (result.IsSuccess)
    {
      return TypedResults.NoContent();
    }

    if (result.Status == ResultStatus.NotFound)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Problem(
      title: "Deactivation failed",
      detail: result.Errors.FirstOrDefault(),
      statusCode: 400
    );
  }
}