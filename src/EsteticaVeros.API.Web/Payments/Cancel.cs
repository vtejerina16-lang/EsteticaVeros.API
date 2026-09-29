using EsteticaVeros.API.UseCases.Payments.Cancel;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Payments;

public class Cancel(IMediator mediator)
  : Endpoint<CancelPaymentRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(CancelPaymentRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Cancelar un pago";
      s.Description = "Permite cancelar un pago que aún no ha sido completado. El pago debe estar en estado 'Pendiente' para ser elegible para cancelación. Se requiere una razón para la cancelación.";
      s.ExampleRequest = new CancelPaymentRequest
      {
        PaymentId = 1,
        Reason = "Pago duplicado"
      };
      s.Responses[204] = "Pago cancelado exitosamente";
      s.Responses[404] = "Pago no encontrado";
      s.Responses[400] = "Pago no puede ser cancelado (ya está completado)";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Admin o Manager)";
    });

    Tags("Payments");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(CancelPaymentRequest req, CancellationToken ct)
  {
    var command = new CancelPaymentCommand(req.PaymentId, req.Reason);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}