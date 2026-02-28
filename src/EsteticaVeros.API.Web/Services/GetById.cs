using EsteticaVeros.API.UseCases.Services;
using EsteticaVeros.API.UseCases.Services.Get;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Services;

public class GetById(IMediator mediator)
  : Endpoint<GetServiceByIdRequest,
             Results<Ok<ServiceDetailResponse>,
                     NotFound,
                     ProblemHttpResult>,
             GetServiceByIdMapper>
{
  public override void Configure()
  {
    Get(GetServiceByIdRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Obtener servicio por ID";
      s.Description = "Recupera los detalles de un servicio específico utilizando su identificador único. Incluye información como número de servicio, tipo de servicio, nombre, descripción, duración, precio base, precio original, porcentaje de descuento, puntos de lealtad ganados, si requiere depósito, monto del depósito, estado activo, destacado, orden de visualización, URL de la imagen, notas, instrucciones de preparación y contraindicaciones.";
      s.ExampleRequest = new GetServiceByIdRequest { ServiceId = 1 };
      // s.ResponseExamples[200] = new ServiceDetailResponse(
      //   1,
      //   "SRV-0001",
      //   "Facial",
      //   "Facial Rejuvenecedor",
      //   1,
      //   "Rejuvenecimiento facial completo",
      //   "Un tratamiento facial que incluye limpieza profunda, exfoliación, extracción de impurezas, masaje facial y aplicación de mascarilla personalizada para revitalizar la piel y mejorar su apariencia.",
      //   "Limpieza profunda, exfoliación, extracción de impurezas, masaje facial, mascarilla personalizada",
      //   TimeSpan.FromMinutes(90),
      //   150.00m,
      //   200.00m,
      //   25.0m,
      //   50,
      //   true,
      //   50.00m,
      //   true,
      //   false,
      //   1,
      //   "https://example.com/images/facial-rejuvenecedor.jpg",
      //   "Ideal para todo tipo de pieles. Se recomienda realizarlo cada 4-6 semanas para mantener resultados óptimos.",
      //   "Evitar exposición solar intensa después del tratamiento. No recomendado para personas con piel extremadamente sensible o con ciertas condiciones dermatológicas.",
      //   new DateTime(2024, 1, 1),
      //   new DateTime(2024, 1, 15),
      //   new List<ServiceItemDTO>
      //   {
      //     new ServiceItemDTO
      //     {
      //       Id = 1,
      //       ServiceId = 1,
      //       Name = "Limpieza Profunda",
      //       Description = "Elimina impurezas y células muertas para preparar la piel para el tratamiento.",
      //       Duration = TimeSpan.FromMinutes(20),
      //       Price = 50.00m,
      //       CreatedAt = new DateTime(2024, 1, 1),
      //       UpdatedAt = new DateTime(2024, 1, 15)
      //     }
      //   }
      // );

      s.Responses[200] = "Service found";
      s.Responses[404] = "Service not found";
      s.Responses[401] = "Not authenticated";
    });

    Tags("Services");

    Description(builder => builder
      .Accepts<GetServiceByIdRequest>()
      .Produces<ServiceDetailResponse>(200, "application/json")
      .ProducesProblem(401)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<ServiceDetailResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetServiceByIdRequest request, CancellationToken ct)
  {
    var result = await mediator.Send(new GetServiceQuery(request.ServiceId), ct);
    return result.ToGetByIdResult(Map.FromEntity);
  }
}

public sealed class GetServiceByIdMapper
  : Mapper<GetServiceByIdRequest, ServiceDetailResponse, ServiceDTO>
{
  public override ServiceDetailResponse FromEntity(ServiceDTO e)
    => new(
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
    );
}