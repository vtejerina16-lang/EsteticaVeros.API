using EsteticaVeros.API.UseCases.ServiceCategories.Delete;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class Delete(IMediator mediator)
  : Endpoint<DeleteServiceCategoryRequest,
             Results<NoContent,
                     NotFound,
                     ProblemHttpResult>>
{
  public override void Configure()
  {
    Delete(DeleteServiceCategoryRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Eliminar una categoría de servicio";
      s.Description = "Desactiva una categoría de servicio (soft delete).";
      s.ExampleRequest = new DeleteServiceCategoryRequest { CategoryId = 1 };

      s.Responses[204] = "Categoría desactivada exitosamente";
      s.Responses[404] = "Categoría no encontrada";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administradores)";
    });

    Tags("Service Categories");

    Description(builder => builder
      .Accepts<DeleteServiceCategoryRequest>()
      .Produces(204)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeleteServiceCategoryRequest req, CancellationToken ct)
  {
    var command = new DeleteServiceCategoryCommand(req.CategoryId);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}