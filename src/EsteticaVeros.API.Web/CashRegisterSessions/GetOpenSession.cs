using EsteticaVeros.API.UseCases.CashRegisterSessions;
using EsteticaVeros.API.UseCases.CashRegisterSessions.GetOpenSession;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class GetOpenSession(IMediator mediator)
  : Endpoint<GetOpenSessionRequest, Ok<GetOpenSessionResponse>>
{
  public override void Configure()
  {
    Get(GetOpenSessionRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Obtener sesión abierta de caja";
      s.Description = "Obtiene la sesión abierta actual para un registrador de caja específico";
      s.Responses[200] = "Devuelve la sesión abierta o nulo si no hay ninguna sesión abierta";
    });

    Tags("CashRegisterSessions");
  }

  public override async Task<Ok<GetOpenSessionResponse>>
    ExecuteAsync(GetOpenSessionRequest req, CancellationToken ct)
  {
    var query = new GetOpenSessionQuery(req.CashRegisterId);
    var result = await mediator.Send(query, ct);

    return TypedResults.Ok(new GetOpenSessionResponse(result.Value));
  }
}