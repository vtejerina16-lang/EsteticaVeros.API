using EsteticaVeros.API.UseCases.PaymentMethods.Activate;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class Activate(IMediator mediator)
  : Endpoint<ActivatePaymentMethodRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(ActivatePaymentMethodRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Activar método de pago";
      s.Description = "Activa un método de pago previamente desactivado en el sistema";
      s.Responses[204] = "Método de pago activado exitosamente";
      s.Responses[404] = "Método de pago no encontrado";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administrador o Gerente)";
    });

    Tags("PaymentMethods");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(ActivatePaymentMethodRequest req, CancellationToken ct)
  {
    var command = new ActivatePaymentMethodCommand(req.PaymentMethodId);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}