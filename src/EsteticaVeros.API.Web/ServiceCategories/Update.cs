using EsteticaVeros.API.UseCases.ServiceCategories;
using EsteticaVeros.API.UseCases.ServiceCategories.Update;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class Update(IMediator mediator)
  : Endpoint<UpdateServiceCategoryRequest,
             Results<Ok<UpdateServiceCategoryResponse>,
                     NotFound,
                     ProblemHttpResult>,
             UpdateServiceCategoryMapper>
{
  public override void Configure()
  {
    Put(UpdateServiceCategoryRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Actualiza una categoría de servicio";
      s.Description = "Actualiza una categoría de servicio existente.";
      s.ExampleRequest = new UpdateServiceCategoryRequest
      {
        CategoryId = 1,
        Name = "Faciales",
        Description = "Tratamientos para el cuidado de la piel del rostro",
        DisplayOrder = 1,
        IconName = "facial-icon",
        ColorCode = "#FF5733",
        IsActive = true
      };
      s.ResponseExamples[200] = new UpdateServiceCategoryResponse(
        new ServiceCategoryDetailResponse(
          1,
          "CAT-0001",
          "Faciales",
          "Tratamientos para el cuidado de la piel del rostro",
          1,
          "facial-icon",
          "#FF5733",
          true,
          DateTime.UtcNow.AddMonths(-2),
          DateTime.UtcNow
        )
      );

      s.Responses[200] = "Categoría actualizada exitosamente";
      s.Responses[404] = "Categoría no encontrada";
      s.Responses[400] = "Datos de entrada inválidos";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administradores o Gerentes)";
    });

    Tags("Service Categories");

    Description(builder => builder
      .Accepts<UpdateServiceCategoryRequest>("application/json")
      .Produces<UpdateServiceCategoryResponse>(200, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<UpdateServiceCategoryResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdateServiceCategoryRequest request, CancellationToken ct)
  {
    var command = new UpdateServiceCategoryCommand(
      request.CategoryId,
      request.Name,
      request.Description,
      request.DisplayOrder,
      request.IconName,
      request.ColorCode,
      request.IsActive
    );

    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class UpdateServiceCategoryMapper
  : Mapper<UpdateServiceCategoryRequest, UpdateServiceCategoryResponse, ServiceCategoryDTO>
{
  public override UpdateServiceCategoryResponse FromEntity(ServiceCategoryDTO e)
    => new(new ServiceCategoryDetailResponse(
      e.Id,
      e.CategoryNumber,
      e.Name,
      e.Description,
      e.DisplayOrder,
      e.IconName,
      e.ColorCode,
      e.IsActive,
      e.CreatedAt,
      e.UpdatedAt
    ));
}