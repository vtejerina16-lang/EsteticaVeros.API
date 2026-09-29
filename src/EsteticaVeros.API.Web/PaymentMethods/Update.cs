using EsteticaVeros.API.UseCases.PaymentMethods;
using EsteticaVeros.API.UseCases.PaymentMethods.Update;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class Update(IMediator mediator)
  : Endpoint<UpdatePaymentMethodRequest,
             Results<Ok<PaymentMethodDTO>, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Put(UpdatePaymentMethodRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Actualizar método de pago";
      s.Description = "Actualiza la información del método de pago";
      s.Responses[200] = "Método de pago actualizado exitosamente";
      s.Responses[404] = "Método de pago no encontrado";
      s.Responses[400] = "Entrada inválida";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administrador o Gerente)";
    });

    Tags("PaymentMethods");
  }

  public override async Task<Results<Ok<PaymentMethodDTO>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdatePaymentMethodRequest req, CancellationToken ct)
  {
    var command = new UpdatePaymentMethodCommand(
      req.PaymentMethodId, 
      req.Name, 
      req.Type, 
      req.RequiresCashRegister
    );
    
    var result = await mediator.Send(command, ct);

    return result.ToUpdateResult(dto => dto);
  }
}