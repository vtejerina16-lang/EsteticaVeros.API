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
      // s.ExampleRequest = new UpdateServiceRequest
      // {
      //   ServiceId = 1,
      //   Name = "Corte de cabello",
      //   Description = "Un corte de cabello profesional para hombres y mujeres.",
      //   ShortDescription = "Corte de cabello profesional",
      //   Duration = TimeSpan.FromMinutes(30),
      //   BasePrice = 20.00m,
      //   OriginalPrice = 25.00m,
      //   DiscountPercentage = 20,
      //   LoyaltyPointsEarned = 10,
      //   RequiresDeposit = false,
      //   DepositAmount = null,
      //   IsActive = true,
      //   IsFeatured = true,
      //   DisplayOrder = 1,
      //   ImageUrl = "https://example.com/images/corte-cabello.jpg",
      //   Notes = "Recomienda reservar con anticipación los fines de semana.",
      //   PreparationInstructions =
      //     "Lavar el cabello antes del servicio. Evitar usar productos pesados.",
      //   Contraindications =
      //     "No recomendado para personas con cuero cabelludo sensible o heridas abiertas.",
      //   ChildServiceIds = new List<int> { 2, 3 }
      // };
      // s.ResponseExamples[200] = new UpdateServiceResponse(
      //   new ServiceDetailResponse(
      //     1,
      //     "SRV-0001",
      //     "Corte",
      //     "Corte de cabello",
      //     1,
      //     "Corte de cabello",
      //     "Un corte de cabello profesional para hombres y mujeres.",
      //     "Corte de cabello profesional",
      //     TimeSpan.FromMinutes(30),
      //     20.00m,
      //     25.00m,
      //     20,
      //     10,
      //     false,
      //     null,
      //     true,
      //     true,
      //     1,
      //     "https://example.com/images/corte-cabello.jpg",
      //     "Recomienda reservar con anticipación los fines de semana.",
      //     "Lavar el cabello antes del servicio. Evitar usar productos pesados.",
      //     "No recomendado para personas con cuero cabelludo sensible o heridas abiertas.",
      //     DateTime.UtcNow.AddMonths(-2),
      //     DateTime.UtcNow,
      //     new List<ServiceItemResponse>
      //     {
      //       new ServiceItemResponse(2, "SRV-0002", "Lavado de cabello", TimeSpan.FromMinutes(15)),
      //       new ServiceItemResponse(3, "SRV-0003", "Peinado", TimeSpan.FromMinutes(20))
      //     }
      //   )
      // );

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
      request.ServiceId,
      request.Name,
      request.Description,
      request.ShortDescription,
      request.Duration,
      request.BasePrice,
      request.OriginalPrice,
      request.DiscountPercentage,
      request.LoyaltyPointsEarned,
      request.RequiresDeposit,
      request.DepositAmount,
      request.IsActive,
      request.IsFeatured,
      request.DisplayOrder,
      request.ImageUrl,
      request.Notes,
      request.PreparationInstructions,
      request.Contraindications,
      request.ChildServiceIds
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