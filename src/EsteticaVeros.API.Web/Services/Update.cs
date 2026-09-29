using EsteticaVeros.API.UseCases.Services;
using EsteticaVeros.API.UseCases.Services.Update;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Services;

public class Update(IMediator mediator)
    : Endpoint<UpdateServiceRequest,
              Results<Ok<UpdateServiceResponse>,
                      NotFound,
                      ProblemHttpResult>,
              UpdateServiceMapper>
{
  public override void Configure()
  {
    Put(UpdateServiceRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Actualiza un servicio";
      s.Description = "Actualiza los datos de un servicio existente.";

      s.Responses[200] = "Servicio actualizado exitosamente";
      s.Responses[404] = "Servicio no encontrado";
      s.Responses[400] = "Datos de entrada inválidos";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administradores y Gerentes)";
    });

    Tags("Services");

    Description(builder => builder
      .Accepts<UpdateServiceRequest>("application/json")
      .Produces<UpdateServiceResponse>(200, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<UpdateServiceResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdateServiceRequest request, CancellationToken ct)
  {
    var command = new UpdateServiceCommand(
      ServiceId: request.ServiceId,
      CategoryId: request.CategoryId,
      Name: request.Name,
      Description: request.Description,
      ShortDescription: request.ShortDescription,
      Duration: request.Duration,
      BasePrice: request.BasePrice,
      OriginalPrice: request.OriginalPrice,
      DiscountPercentage: request.DiscountPercentage,
      LoyaltyPointsEarned: request.LoyaltyPointsEarned,
      RequiresDeposit: request.RequiresDeposit,
      DepositAmount: request.DepositAmount,
      IsActive: request.IsActive,
      IsFeatured: request.IsFeatured,
      DisplayOrder: request.DisplayOrder,
      ImageUrl: request.ImageUrl,
      Notes: request.Notes,
      PreparationInstructions: request.PreparationInstructions,
      Contraindications: request.Contraindications,
      ChildServiceIds: request.ChildServiceIds
    );

    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class UpdateServiceMapper
  : Mapper<UpdateServiceRequest, UpdateServiceResponse, ServiceDTO>
{
  public override UpdateServiceResponse FromEntity(ServiceDTO e)
    => new(new ServiceDetailResponse(
      e.Id,
      e.ServiceNumber,
      e.ServiceType,
      e.ServiceTypeName,
      e.CategoryId,
      e.Name,
      e.Description,
      e.ShortDescription,
      e.Duration,
      e.BasePrice,
      e.OriginalPrice,
      e.DiscountPercentage,
      e.LoyaltyPointsEarned,
      e.RequiresDeposit,
      e.DepositAmount,
      e.IsActive,
      e.IsFeatured,
      e.DisplayOrder,
      e.ImageUrl,
      e.Notes,
      e.PreparationInstructions,
      e.Contraindications,
      e.CreatedAt,
      e.UpdatedAt,
      e.ServiceItems
    ));
}