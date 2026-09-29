using EsteticaVeros.API.UseCases.CashRegisters;
using EsteticaVeros.API.UseCases.CashRegisters.List;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisters;

public class List(IMediator mediator)
  : Endpoint<ListCashRegistersRequest, Ok<ListCashRegistersResponse>>
{
  public override void Configure()
  {
    Get(ListCashRegistersRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Lista cajas registradoras";
      s.Description = "Obtiene una lista de cajas registradoras, con la opción de filtrar por estado activo.";
      s.Responses[200] = "Caja registradora encontrada";
    });

    Tags("CashRegisters");
  }

  public override async Task<Ok<ListCashRegistersResponse>>
    ExecuteAsync(ListCashRegistersRequest req, CancellationToken ct)
  {
    var query = new ListCashRegistersQuery(req.IsActive);
    var result = await mediator.Send(query, ct);

    return result.ToOkOnlyResult(dtos => new ListCashRegistersResponse(dtos));
  }
}