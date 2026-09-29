using EsteticaVeros.API.UseCases.CashReconciliations;
using EsteticaVeros.API.UseCases.CashReconciliations.GetById;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class GetById(IMediator mediator)
  : Endpoint<GetCashReconciliationByIdRequest,
             Results<Ok<CashReconciliationDTO>, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetCashReconciliationByIdRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Obtener conciliación de caja por ID";
      s.Description = "Obtiene los detalles de una conciliación de caja específica utilizando su ID.";
      s.Responses[200] = "Conciliación encontrada";
      s.Responses[404] = "Conciliación no encontrada";
    });

    Tags("CashReconciliations");
  }

  public override async Task<Results<Ok<CashReconciliationDTO>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetCashReconciliationByIdRequest req, CancellationToken ct)
  {
    var query = new GetCashReconciliationByIdQuery(req.ReconciliationId);
    var result = await mediator.Send(query, ct);

    return result.ToGetByIdResult(dto => dto);
  }
}