using EsteticaVeros.API.UseCases.CashRegisters;
using EsteticaVeros.API.UseCases.CashRegisters.GetById;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisters;

public class GetById(IMediator mediator)
  : Endpoint<GetCashRegisterByIdRequest,
             Results<Ok<CashRegisterDTO>, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetCashRegisterByIdRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Obtener caja por ID";
      s.Description = "Obtiene los detalles de una caja registradora específica utilizando su ID.";
      s.Responses[200] = "Caja encontrada";
      s.Responses[404] = "Caja no encontrada";
    });

    Tags("CashRegisters");
  }

  public override async Task<Results<Ok<CashRegisterDTO>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetCashRegisterByIdRequest req, CancellationToken ct)
  {
    var query = new GetCashRegisterByIdQuery(req.CashRegisterId);
    var result = await mediator.Send(query, ct);

    return result.ToGetByIdResult(dto => dto);
  }
}