using EsteticaVeros.API.UseCases.CashRegisterSessions;
using EsteticaVeros.API.UseCases.CashRegisterSessions.List;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class List(IMediator mediator)
  : Endpoint<ListCashRegisterSessionsRequest, Ok<ListCashRegisterSessionsResponse>>
{
  public override void Configure()
  {
    Get(ListCashRegisterSessionsRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Listar sesiones de caja registradora";
      s.Description = "Recupera una lista de sesiones de caja registradora con filtros opcionales";
      s.Responses[200] = "Sesiones recuperadas exitosamente";
    });

    Tags("CashRegisterSessions");
  }

  public override async Task<Ok<ListCashRegisterSessionsResponse>>
    ExecuteAsync(ListCashRegisterSessionsRequest req, CancellationToken ct)
  {
    var query = new ListCashRegisterSessionsQuery(
      req.CashRegisterId,
      req.StartDate,
      req.EndDate,
      req.Status
    );

    var result = await mediator.Send(query, ct);

    return result.ToOkOnlyResult(dtos => new ListCashRegisterSessionsResponse(dtos));
  }
}