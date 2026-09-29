using EsteticaVeros.API.UseCases.CashReconciliations.Approve;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class Approve(IMediator mediator)
  : Endpoint<ApproveCashReconciliationRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(ApproveCashReconciliationRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Aprueba un arqueo de caja";
      s.Description = "Aprueba un arqueo de caja pendiente y marca la sesión como reconciliada";
      s.ExampleRequest = new ApproveCashReconciliationRequest
      {
        ReconciliationId = 1,
        Notes = "Aprobado - diferencia justificada"
      };
      s.Responses[204] = "Arqueo aprobado exitosamente";
      s.Responses[404] = "Arqueo no encontrado";
      s.Responses[400] = "Arqueo no está en estado pendiente";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador)";
    });

    Tags("CashReconciliations");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(ApproveCashReconciliationRequest req, CancellationToken ct)
  {
    var userId = User.GetUserId();
    if (!userId.HasValue)
    {
      return TypedResults.Problem(
        title: "Authentication required",
        detail: "User ID is required to approve reconciliation",
        statusCode: 401
      );
    }

    var command = new ApproveCashReconciliationCommand(
      req.ReconciliationId,
      userId.Value,
      req.Notes
    );

    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}