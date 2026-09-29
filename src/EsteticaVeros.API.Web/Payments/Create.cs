using EsteticaVeros.API.UseCases.Payments.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Payments;

public class Create(IMediator mediator)
  : Endpoint<CreatePaymentRequest,
             Results<Created<CreatePaymentResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreatePaymentMapper>
{
  public override void Configure()
  {
    Post(CreatePaymentRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Crear un nuevo pago";
      s.Description = "Crea un nuevo registro de pago para un cliente. Si el método de pago requiere caja registradora, se crea automáticamente un movimiento de efectivo.";
      s.ExampleRequest = new CreatePaymentRequest
      {
        CustomerId = 1,
        PaymentMethodId = 1,
        Amount = 150.00m,
        AppointmentId = 5,
        CashRegisterSessionId = 1,
        Reference = "Pago en efectivo"
      };
      s.Responses[201] = "Pago creado exitosamente";
      s.Responses[400] = "Entrada inválida o error de validación";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administrador o Gerente)";
    });

    Tags("Payments");
  }

  public override async Task<Results<Created<CreatePaymentResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreatePaymentRequest req, CancellationToken ct)
  {
    var userId = User.GetUserId();

    var command = new CreatePaymentCommand(
      req.CustomerId,
      req.PaymentMethodId,
      req.Amount,
      req.AppointmentId,
      req.CashRegisterSessionId,
      req.Reference,
      req.Notes,
      userId
    );

    var result = await mediator.Send(command, ct);

    return result.ToCreatedResult(
      id => CreatePaymentRequest.BuildRoute(id),
      id => new CreatePaymentResponse(id)
    );
  }
}

public sealed class CreatePaymentMapper
  : Mapper<CreatePaymentRequest, CreatePaymentResponse, int>
{
  public override CreatePaymentResponse FromEntity(int id)
    => new(id);
}