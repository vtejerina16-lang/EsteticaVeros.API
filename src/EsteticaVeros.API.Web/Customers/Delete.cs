using EsteticaVeros.API.UseCases.Customers.Delete;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Customers;

public class Delete(IMediator mediator)
  : Endpoint<DeleteCustomerRequest,
             Results<NoContent,
                     NotFound,
                     ProblemHttpResult>>
{
  public override void Configure()
  {
    Delete(DeleteCustomerRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Eliminar un cliente";
      s.Description = "Desactiva un cliente (soft delete).";
      s.ExampleRequest = new DeleteCustomerRequest { CustomerId = 1 };

      s.Responses[204] = "Cliente desactivado exitosamente";
      s.Responses[404] = "Cliente no encontrado";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administradores y Gerentes)";
    });

    Tags("Customers");

    Description(builder => builder
      .Accepts<DeleteCustomerRequest>()
      .Produces(204)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeleteCustomerRequest req, CancellationToken ct)
  {
    var command = new DeleteCustomerCommand(req.CustomerId);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}