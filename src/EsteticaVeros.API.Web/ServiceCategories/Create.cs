using EsteticaVeros.API.UseCases.ServiceCategories;
using EsteticaVeros.API.UseCases.ServiceCategories.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class Create(IMediator mediator)
  : Endpoint<CreateServiceCategoryRequest,
             Results<Created<CreateServiceCategoryResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreateServiceCategoryMapper>
{
  public override void Configure()
  {
    Post(CreateServiceCategoryRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Crear categoría de servicio";
      s.Description = "Crea una nueva categoría de servicio en el sistema.";
      s.ExampleRequest = new CreateServiceCategoryRequest
      {
        Name = "Faciales",
        Description = "Tratamientos faciales para el cuidado de la piel",
        DisplayOrder = 1,
        IconName = "fa-solid fa-face-smile",
        ColorCode = "#FF5733"
      };
      s.ResponseExamples[201] = new CreateServiceCategoryResponse(
        new ServiceCategoryRecord(
          1,
          "CAT-0001",
          "Faciales",
          1,
          "fa-solid fa-face-smile",
          "#FF5733",
          true
        )
      );

      s.Responses[201] = "Categoría creada exitosamente";
      s.Responses[400] = "Error de validación";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administradores o Gerentes)";
    });

    Tags("Service Categories");

    Description(builder => builder
      .Accepts<CreateServiceCategoryRequest>("application/json")
      .Produces<CreateServiceCategoryResponse>(201, "application/json")
      .ProducesProblem(400));
  }

  public override async Task<Results<Created<CreateServiceCategoryResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateServiceCategoryRequest req, CancellationToken ct)
  {
    var command = new CreateServiceCategoryCommand(
      req.Name,
      req.Description,
      req.DisplayOrder,
      req.IconName,
      req.ColorCode,
      User.GetUserId()
    );

    var result = await mediator.Send(command, ct);

    if (!result.IsSuccess)
    {
      return TypedResults.Problem(
        title: "Category creation failed",
        detail: result.Errors.FirstOrDefault(),
        statusCode: 400
      );
    }

    var response = Map.FromEntity(result.Value);
    return TypedResults.Created($"/api/service-categories/{result.Value.Id}", response);
  }
}

public sealed class CreateServiceCategoryMapper
  : Mapper<CreateServiceCategoryRequest, CreateServiceCategoryResponse, ServiceCategoryDTO>
{
  public override CreateServiceCategoryResponse FromEntity(ServiceCategoryDTO e)
    => new(new ServiceCategoryRecord(
      e.Id,
      e.CategoryNumber,
      e.Name,
      e.DisplayOrder,
      e.IconName,
      e.ColorCode,
      e.IsActive
    ));
}