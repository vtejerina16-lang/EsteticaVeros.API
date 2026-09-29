using EsteticaVeros.API.UseCases.CashReconciliations;
using EsteticaVeros.API.UseCases.CashReconciliations.List;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class List(IMediator mediator)
  : Endpoint<ListCashReconciliationsRequest, Ok<ListCashReconciliationsResponse>>
{
  public override void Configure()
  {
    Get(ListCashReconciliationsRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Listar conciliaciones de caja";
      s.Description = "Obtiene una lista de conciliaciones de caja filtradas por sesión de caja, estado y rango de fechas.";
      s.Responses[200] = "Conciliaciones obtenidas exitosamente";
    });

    Tags("CashReconciliations");
  }

  public override async Task<Ok<ListCashReconciliationsResponse>>
    ExecuteAsync(ListCashReconciliationsRequest req, CancellationToken ct)
  {
    var query = new ListCashReconciliationsQuery(
      req.CashRegisterSessionId,
      req.Status,
      req.StartDate,
      req.EndDate
    );

    var result = await mediator.Send(query, ct);

    return result.ToOkOnlyResult(dtos => new ListCashReconciliationsResponse(dtos));
  }
}