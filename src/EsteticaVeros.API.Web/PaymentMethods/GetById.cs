using EsteticaVeros.API.UseCases.PaymentMethods;
using EsteticaVeros.API.UseCases.PaymentMethods.GetById;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class GetById(IMediator mediator)
  : Endpoint<GetPaymentMethodByIdRequest,
             Results<Ok<PaymentMethodDTO>, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetPaymentMethodByIdRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Obtener método de pago por ID";
      s.Description = "Recupera un método de pago por su ID";
      s.Responses[200] = "Método de pago encontrado";
      s.Responses[404] = "Método de pago no encontrado";
    });

    Tags("PaymentMethods");
  }

  public override async Task<Results<Ok<PaymentMethodDTO>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetPaymentMethodByIdRequest req, CancellationToken ct)
  {
    var query = new GetPaymentMethodByIdQuery(req.PaymentMethodId);
    var result = await mediator.Send(query, ct);

    return result.ToGetByIdResult(dto => dto);
  }
}