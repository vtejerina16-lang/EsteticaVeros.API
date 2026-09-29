using EsteticaVeros.API.UseCases.PaymentMethods.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class Create(IMediator mediator)
  : Endpoint<CreatePaymentMethodRequest,
             Results<Created<CreatePaymentMethodResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreatePaymentMethodMapper>
{
  public override void Configure()
  {
    Post(CreatePaymentMethodRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Crear método de pago";
      s.Description = "Crea un nuevo método de pago en el sistema. Tipo 1: Efectivo, Tipo 2: Tarjeta, Tipo 3: Transferencia bancaria.";
      s.ExampleRequest = new CreatePaymentMethodRequest
      {
        Name = "Tarjeta Visa",
        Type = 2,
        RequiresCashRegister = false
      };
      s.Responses[201] = "Método de pago creado exitosamente";
      s.Responses[400] = "Entrada inválida o tipo inválido";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador o Gerente)";
    });

    Tags("PaymentMethods");
  }

  public override async Task<Results<Created<CreatePaymentMethodResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreatePaymentMethodRequest req, CancellationToken ct)
  {
    var command = new CreatePaymentMethodCommand(req.Name, req.Type, req.RequiresCashRegister);
    var result = await mediator.Send(command, ct);

    return result.ToCreatedResult(
      id => CreatePaymentMethodRequest.BuildRoute(id),
      id => new CreatePaymentMethodResponse(id)
    );
  }
}

public sealed class CreatePaymentMethodMapper
  : Mapper<CreatePaymentMethodRequest, CreatePaymentMethodResponse, int>
{
  public override CreatePaymentMethodResponse FromEntity(int id)
    => new(id);
}