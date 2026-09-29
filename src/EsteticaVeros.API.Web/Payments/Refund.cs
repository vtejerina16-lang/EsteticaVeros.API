using EsteticaVeros.API.UseCases.Payments.Refund;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Payments;

public class Refund(IMediator mediator)
  : Endpoint<RefundPaymentRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(RefundPaymentRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Reembolsar un pago";
      s.Description = "Permite reembolsar un pago completado. El pago debe estar en estado 'Completado' para ser elegible para reembolso. Se requiere una razón para el reembolso.";
      s.ExampleRequest = new RefundPaymentRequest
      {
        PaymentId = 1,
        Reason = "Cliente solicitó reembolso"
      };
      s.Responses[204] = "Pago reembolsado exitosamente";
      s.Responses[404] = "Pago no encontrado";
      s.Responses[400] = "Pago no puede ser reembolsado (no está completado)";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Admin o Manager)";
    });

    Tags("Payments");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(RefundPaymentRequest req, CancellationToken ct)
  {
    var command = new RefundPaymentCommand(req.PaymentId, req.Reason);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}