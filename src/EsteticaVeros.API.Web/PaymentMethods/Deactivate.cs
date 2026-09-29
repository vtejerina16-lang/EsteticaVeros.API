using EsteticaVeros.API.UseCases.PaymentMethods.Deactivate;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class Deactivate(IMediator mediator)
  : Endpoint<DeactivatePaymentMethodRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(DeactivatePaymentMethodRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Desactivar método de pago";
      s.Description = "Desactiva un método de pago activo";
      s.Responses[204] = "Método de pago desactivado exitosamente";
      s.Responses[404] = "Método de pago no encontrado";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administrador o Gerente)";
    });

    Tags("PaymentMethods");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeactivatePaymentMethodRequest req, CancellationToken ct)
  {
    var command = new DeactivatePaymentMethodCommand(req.PaymentMethodId);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}