using EsteticaVeros.API.UseCases.CashReconciliations.Reject;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class Reject(IMediator mediator)
  : Endpoint<RejectCashReconciliationRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(RejectCashReconciliationRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Rechaza un arqueo de caja";
      s.Description = "Rechaza un arqueo de caja pendiente con un motivo";
      s.ExampleRequest = new RejectCashReconciliationRequest
      {
        ReconciliationId = 1,
        Reason = "Diferencia muy grande - requiere nuevo conteo"
      };
      s.Responses[204] = "Arqueo rechazado exitosamente";
      s.Responses[404] = "Arqueo no encontrado";
      s.Responses[400] = "Arqueo no está en estado pendiente o se requiere motivo";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador)";
    });

    Tags("CashReconciliations");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(RejectCashReconciliationRequest req, CancellationToken ct)
  {
    var userId = User.GetUserId();
    if (!userId.HasValue)
    {
      return TypedResults.Problem(
        title: "Authentication required",
        detail: "User ID is required to reject reconciliation",
        statusCode: 401
      );
    }

    var command = new RejectCashReconciliationCommand(
      req.ReconciliationId,
      userId.Value,
      req.Reason
    );

    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}