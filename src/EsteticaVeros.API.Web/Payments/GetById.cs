using EsteticaVeros.API.UseCases.Payments;
using EsteticaVeros.API.UseCases.Payments.GetById;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Payments;

public class GetById(IMediator mediator)
  : Endpoint<GetPaymentByIdRequest,
             Results<Ok<PaymentDTO>, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get(GetPaymentByIdRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Obtener pago por ID";
      s.Description = "Recupera los detalles de un pago específico utilizando su ID único.";
      s.Responses[200] = "Pago encontrado y detalles devueltos exitosamente";
      s.Responses[404] = "Pago no encontrado";
    });

    Tags("Payments");
  }

  public override async Task<Results<Ok<PaymentDTO>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetPaymentByIdRequest req, CancellationToken ct)
  {
    var query = new GetPaymentByIdQuery(req.PaymentId);
    var result = await mediator.Send(query, ct);

    return result.ToGetByIdResult(dto => dto);
  }
}