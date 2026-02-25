using EsteticaVeros.API.UseCases.Customers;
using EsteticaVeros.API.UseCases.Customers.Get;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Customers;

public class GetById(IMediator mediator)
  : Endpoint<GetCustomerByIdRequest,
             Results<Ok<CustomerDetailResponse>,
                     NotFound,
                     ProblemHttpResult>,
             GetCustomerByIdMapper>
{
  public override void Configure()
  {
    Get(GetCustomerByIdRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Obtener cliente por ID";
      s.Description = "Recupera los detalles de un cliente específico utilizando su identificador único. Incluye información como número de cliente, nombre completo, número de teléfono, correo electrónico, fecha de nacimiento, género, dirección, alergias, condiciones médicas, medicamentos, asistente preferido, puntos de lealtad, total gastado, total de visitas y fecha de la última visita.";
      s.ExampleRequest = new GetCustomerByIdRequest { CustomerId = 1 };
      s.ResponseExamples[200] = new CustomerDetailResponse(
        1,
        "CUST-0001",
        "Juan",
        "Pérez",
        "Juan Pérez",
        "+1234567890",
        "juan.perez@ejemplo.com",
        new DateTime(1990, 5, 15),
        33,
        "Masculino",
        "Calle Principal 123",
        "Ciudad Ejemplo",
        "Polen de hierba",
        "Hipertensión",
        "Lisinopril",
        2,
        null,
        100,
        0.0m,
        5,
        new DateTime(2024, 1, 15),
        true,
        new DateTime(2024, 1, 1),
        new DateTime(2024, 1, 15)
      );
      s.Responses[200] = "Cliente encontrado";
      s.Responses[404] = "Cliente no encontrado";
      s.Responses[401] = "No autenticado";
    });

    Tags("Customers");

    Description(builder => builder
      .Accepts<GetCustomerByIdRequest>()
      .Produces<CustomerDetailResponse>(200, "application/json")
      .ProducesProblem(401)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<CustomerDetailResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetCustomerByIdRequest request, CancellationToken ct)
  {
    var result = await mediator.Send(new GetCustomerQuery(request.CustomerId), ct);
    return result.ToGetByIdResult(Map.FromEntity);
  }
}

public sealed class GetCustomerByIdMapper
  : Mapper<GetCustomerByIdRequest, CustomerDetailResponse, CustomerDTO>
{
  public override CustomerDetailResponse FromEntity(CustomerDTO e)
    => new(
      e.Id,
      e.CustomerNumber,
      e.FirstName,
      e.LastName,
      e.FullName,
      e.PhoneNumber,
      e.Email,
      e.DateOfBirth,
      e.Age,
      e.Gender,
      e.Address,
      e.City,
      e.Allergies,
      e.MedicalConditions,
      e.Medications,
      e.PreferredAssistantId,
      e.ProfilePictureUrl,
      e.LoyaltyPoints,
      e.TotalSpent,
      e.TotalVisits,
      e.LastVisitDate,
      e.IsActive,
      e.CreatedAt,
      e.UpdatedAt
    );
}