using EsteticaVeros.API.UseCases.CashRegisterSessions.Close;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class Close(IMediator mediator)
  : Endpoint<CloseCashRegisterSessionRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(CloseCashRegisterSessionRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Cerrar sesión de caja registradora";
      s.Description = "Cierra una sesión de caja registradora abierta";
      s.ExampleRequest = new CloseCashRegisterSessionRequest
      {
        SessionId = 1,
        Notes = "Cierre del turno de la mañana"
      };
      s.Responses[204] = "Sesión cerrada exitosamente";
      s.Responses[404] = "Sesión no encontrada";
      s.Responses[400] = "Sesión ya cerrada o estado inválido";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador o Gerente)";
    });

    Tags("CashRegisterSessions");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(CloseCashRegisterSessionRequest req, CancellationToken ct)
  {
    var userId = User.GetUserId();
    if (!userId.HasValue)
    {
      return TypedResults.Problem(
        title: "Authentication required",
        detail: "User ID is required to close a session",
        statusCode: 401
      );
    }

    var command = new CloseCashRegisterSessionCommand(
      req.SessionId,
      userId.Value,
      req.Notes
    );

    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}