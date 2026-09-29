using EsteticaVeros.API.UseCases.CashMovements;
using EsteticaVeros.API.UseCases.CashMovements.List;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashMovements;

public class List(IMediator mediator)
  : Endpoint<ListCashMovementsRequest, Ok<ListCashMovementsResponse>>
{
  public override void Configure()
  {
    Get(ListCashMovementsRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Listar movimientos de caja";
      s.Description = "Obtiene una lista de todos los movimientos de caja con filtros opcionales";
      s.Responses[200] = "Movimientos de caja recuperados exitosamente";
    });

    Tags("CashMovements");
  }

  public override async Task<Ok<ListCashMovementsResponse>>
    ExecuteAsync(ListCashMovementsRequest req, CancellationToken ct)
  {
    var query = new ListCashMovementsQuery(
      req.CashRegisterSessionId,
      req.StartDate,
      req.EndDate,
      req.Type
    );

    var result = await mediator.Send(query, ct);

    return result.ToOkOnlyResult(dtos => new ListCashMovementsResponse(dtos));
  }
}