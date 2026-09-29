using EsteticaVeros.API.UseCases.CashMovements;
using EsteticaVeros.API.UseCases.CashMovements.GetById;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashMovements;

public class GetById(IMediator mediator)
  : Endpoint<GetCashMovementByIdRequest,
             Results<Ok<CashMovementDTO>, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetCashMovementByIdRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Obtiene un movimiento de caja por su ID";
      s.Description = "Obtiene los detalles de un movimiento de caja específico utilizando su ID único.";
      s.Responses[200] = "Movimiento de caja encontrado";
      s.Responses[404] = "Movimiento de caja no encontrado";
    });

    Tags("CashMovements");
  }

  public override async Task<Results<Ok<CashMovementDTO>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetCashMovementByIdRequest req, CancellationToken ct)
  {
    var query = new GetCashMovementByIdQuery(req.MovementId);
    var result = await mediator.Send(query, ct);

    return result.ToGetByIdResult(dto => dto);
  }
}