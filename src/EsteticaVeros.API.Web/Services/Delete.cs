using EsteticaVeros.API.UseCases.Services.Delete;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Services;

public class Delete(IMediator mediator)
  : Endpoint<DeleteServiceRequest,
             Results<NoContent,
                     NotFound,
                     ProblemHttpResult>>
{
  public override void Configure()
  {
    Delete(DeleteServiceRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Elimina un servicio";
      s.Description = "Desactiva un servicio (soft delete).";
      s.ExampleRequest = new DeleteServiceRequest { ServiceId = 1 };

      s.Responses[204] = "Servicio desactivado exitosamente";
      s.Responses[404] = "Servicio no encontrado";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administradores)";
    });

    Tags("Services");

    Description(builder => builder
      .Accepts<DeleteServiceRequest>()
      .Produces(204)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeleteServiceRequest req, CancellationToken ct)
  {
    var command = new DeleteServiceCommand(req.ServiceId);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}