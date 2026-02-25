using EsteticaVeros.API.UseCases.Customers;
using EsteticaVeros.API.UseCases.Customers.Update;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Customers;

public class Update(IMediator mediator)
  : Endpoint<UpdateCustomerRequest,
             Results<Ok<UpdateCustomerResponse>,
                     NotFound,
                     ProblemHttpResult>,
             UpdateCustomerMapper>
{
  public override void Configure()
  {
    Put(UpdateCustomerRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Actualiza un cliente";
      s.Description = "Actualiza los datos de un cliente existente.";
      s.ExampleRequest = new UpdateCustomerRequest
      {
        CustomerId = 1,
        FirstName = "María",
        LastName = "Gómez",
        PhoneNumber = "98765432",
        Gender = 2,
        Email = "maria.gomez@ejemplo.com",
        DateOfBirth = new DateTime(1990, 5, 15),
        Address = "Calle Principal 123",
        City = "Ciudad Ejemplo",
        Allergies = "Polen",
        MedicalConditions = "Hipertensión",
        Medications = "Lisinopril",
        PreferredAssistantId = 2,
        IsActive = true
      };
      s.ResponseExamples[200] = new UpdateCustomerResponse(
        new CustomerDetailResponse(
          1,
          "CUST-0001",
          "María",
          "Gómez",
          "María Gómez",
          "98765432",
          "maria.gomez@ejemplo.com",
          new DateTime(1990, 5, 15),
          33,
          "Femenino",
          "Calle Principal 123",
          "Ciudad Ejemplo",
          "Polen",
          "Hipertensión",
          "Lisinopril",
          2,
          null,
          0,
          0.0m,
          0,
          null,
          true,
          DateTime.UtcNow,
          DateTime.UtcNow
        )
      );
      
      s.Responses[200] = "Cliente actualizado exitosamente";
      s.Responses[404] = "Cliente no encontrado";
      s.Responses[400] = "Datos de entrada inválidos";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo administradores o gerentes)";
    });

    Tags("Customers");

    Description(builder => builder
      .Accepts<UpdateCustomerRequest>("application/json")
      .Produces<UpdateCustomerResponse>(200, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<UpdateCustomerResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdateCustomerRequest request, CancellationToken ct)
  {
    var command = new UpdateCustomerCommand(
      request.CustomerId,
      request.FirstName,
      request.LastName,
      request.PhoneNumber,
      request.Gender,
      request.Email,
      request.DateOfBirth,
      request.Address,
      request.City,
      request.Allergies,
      request.MedicalConditions,
      request.Medications,
      request.PreferredAssistantId,
      request.IsActive
    );

    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class UpdateCustomerMapper
  : Mapper<UpdateCustomerRequest, UpdateCustomerResponse, CustomerDTO>
{
  public override UpdateCustomerResponse FromEntity(CustomerDTO e)
    => new(new CustomerDetailResponse(
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
    ));
}