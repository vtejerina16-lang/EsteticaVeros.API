using EsteticaVeros.API.UseCases.PaymentMethods;
using EsteticaVeros.API.UseCases.PaymentMethods.List;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class List(IMediator mediator)
  : Endpoint<ListPaymentMethodsRequest, Ok<ListPaymentMethodsResponse>>
{
  public override void Configure()
  {
    Get(ListPaymentMethodsRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Listar métodos de pago";
      s.Description = "Recupera una lista de todos los métodos de pago con filtros opcionales";
      s.Responses[200] = "Métodos de pago recuperados exitosamente";
    });

    Tags("PaymentMethods");
  }

  public override async Task<Ok<ListPaymentMethodsResponse>>
    ExecuteAsync(ListPaymentMethodsRequest req, CancellationToken ct)
  {
    var query = new ListPaymentMethodsQuery(req.IsActive, req.RequiresCashRegister);
    var result = await mediator.Send(query, ct);

    return result.ToOkOnlyResult(dtos => new ListPaymentMethodsResponse(dtos));
  }
}