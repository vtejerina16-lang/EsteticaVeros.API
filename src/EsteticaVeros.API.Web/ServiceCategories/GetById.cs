using EsteticaVeros.API.UseCases.ServiceCategories;
using EsteticaVeros.API.UseCases.ServiceCategories.Get;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class GetById(IMediator mediator)
  : Endpoint<GetServiceCategoryByIdRequest,
             Results<Ok<ServiceCategoryDetailResponse>,
                     NotFound,
                     ProblemHttpResult>,
             GetServiceCategoryByIdMapper>
{
  public override void Configure()
  {
    Get(GetServiceCategoryByIdRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Obtener categoría de servicio por ID";
      s.Description = "Recupera los detalles de una categoría de servicio específica utilizando su identificador único. Incluye información como número de categoría, nombre, descripción, orden de visualización, nombre del ícono, código de color, estado activo, fecha de creación y fecha de última actualización.";
      s.ExampleRequest = new GetServiceCategoryByIdRequest { CategoryId = 1 };
      s.ResponseExamples[200] = new ServiceCategoryDetailResponse(
        1,
        "CAT-0001",
        "Corte de cabello",
        "Servicios de corte de cabello para hombres y mujeres.",
        1,
        "scissors",
        "#FF5733",
        true,
        new DateTime(2024, 1, 1),
        new DateTime(2024, 1, 15)
      );

      s.Responses[200] = "Categoría encontrada";
      s.Responses[404] = "Categoría no encontrada";
      s.Responses[401] = "No autenticado";
    });

    Tags("Service Categories");

    Description(builder => builder
      .Accepts<GetServiceCategoryByIdRequest>()
      .Produces<ServiceCategoryDetailResponse>(200, "application/json")
      .ProducesProblem(401)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<ServiceCategoryDetailResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetServiceCategoryByIdRequest request, CancellationToken ct)
  {
    var result = await mediator.Send(new GetServiceCategoryQuery(request.CategoryId), ct);
    return result.ToGetByIdResult(Map.FromEntity);
  }
}

public sealed class GetServiceCategoryByIdMapper
  : Mapper<GetServiceCategoryByIdRequest, ServiceCategoryDetailResponse, ServiceCategoryDTO>
{
  public override ServiceCategoryDetailResponse FromEntity(ServiceCategoryDTO e)
    => new(
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
    );
}