using EsteticaVeros.API.UseCases.CashRegisterSessions;
using EsteticaVeros.API.UseCases.CashRegisterSessions.GetById;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class GetById(IMediator mediator)
  : Endpoint<GetCashRegisterSessionByIdRequest,
             Results<Ok<CashRegisterSessionDTO>, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetCashRegisterSessionByIdRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Obtener sesión de caja registradora por ID";
      s.Description = "Recupera una sesión de caja registradora por su ID";
      s.Responses[200] = "Sesión encontrada";
      s.Responses[404] = "Sesión no encontrada";
    });

    Tags("CashRegisterSessions");
  }

  public override async Task<Results<Ok<CashRegisterSessionDTO>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetCashRegisterSessionByIdRequest req, CancellationToken ct)
  {
    var query = new GetCashRegisterSessionByIdQuery(req.SessionId);
    var result = await mediator.Send(query, ct);

    return result.ToGetByIdResult(dto => dto);
  }
}