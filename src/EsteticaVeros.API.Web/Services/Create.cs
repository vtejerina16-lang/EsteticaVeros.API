using EsteticaVeros.API.UseCases.Services;
using EsteticaVeros.API.UseCases.Services.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Services;

public class Create(IMediator mediator)
  : Endpoint<CreateServiceRequest,
             Results<Created<CreateServiceResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreateServiceMapper>
{
  public override void Configure()
  {
    Post(CreateServiceRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Crear Servicio";
      s.Description = "Crea un nuevo servicio en el sistema. El servicio se identifica por su nombre, por lo que no se pueden crear servicios con el mismo nombre.";
      
      s.Responses[201] = "Servicio creado exitosamente";
      s.Responses[400] = "Error de validación";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administradores y Gerentes)";
    });

    Tags("Services");

    Description(builder => builder
      .Accepts<CreateServiceRequest>("application/json")
      .Produces<CreateServiceResponse>(201, "application/json")
      .ProducesProblem(400));
  }

  public override async Task<Results<Created<CreateServiceResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateServiceRequest req, CancellationToken ct)
  {
    var command = new CreateServiceCommand(
      req.ServiceType,
      req.CategoryId,
      req.Name,
      req.Description,
      req.ShortDescription,
      req.Duration,
      req.BasePrice,
      req.OriginalPrice,
      req.DiscountPercentage,
      req.LoyaltyPointsEarned,
      req.RequiresDeposit,
      req.DepositAmount,
      req.IsFeatured,
      req.DisplayOrder,
      req.ImageUrl,
      req.Notes,
      req.PreparationInstructions,
      req.Contraindications,
      req.ChildServiceIds,
      User.GetUserId()
    );

    var result = await mediator.Send(command, ct);

    if (!result.IsSuccess)
    {
      return TypedResults.Problem(
        title: "Service creation failed",
        detail: result.Errors.FirstOrDefault(),
        statusCode: 400
      );
    }

    var response = Map.FromEntity(result.Value);
    return TypedResults.Created($"/api/services/{result.Value.Id}", response);
  }
}

public sealed class CreateServiceMapper
  : Mapper<CreateServiceRequest, CreateServiceResponse, ServiceDTO>
{
  public override CreateServiceResponse FromEntity(ServiceDTO e)
    => new(new ServiceRecord(
      e.Id,
      e.ServiceNumber,
      e.Name,
      e.ServiceTypeName,
      e.Duration,
      e.BasePrice,
      e.IsActive
    ));
}