using EsteticaVeros.API.UseCases.Payments;
using EsteticaVeros.API.UseCases.Payments.List;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Payments;

public class List(IMediator mediator)
  : Endpoint<ListPaymentsRequest, Ok<ListPaymentsResponse>>
{
  public override void Configure()
  {
    Get(ListPaymentsRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Lista de pagos";
      s.Description = "Recupera una lista de todos los pagos con filtros opcionales";
      s.Responses[200] = "Pagos recuperados exitosamente";
    });

    Tags("Payments");
  }

  public override async Task<Ok<ListPaymentsResponse>>
    ExecuteAsync(ListPaymentsRequest req, CancellationToken ct)
  {
    var query = new ListPaymentsQuery(
      req.CustomerId,
      req.AppointmentId,
      req.CashRegisterSessionId,
      req.StartDate,
      req.EndDate,
      req.Status
    );

    var result = await mediator.Send(query, ct);

    return result.ToOkOnlyResult(dtos => new ListPaymentsResponse(dtos));
  }
}