using EsteticaVeros.API.UseCases.Customers;
using EsteticaVeros.API.UseCases.Customers.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Customers;

public class Create(IMediator mediator)
  : Endpoint<CreateCustomerRequest,
             Results<Created<CreateCustomerResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreateCustomerMapper>
{
  public override void Configure()
  {
    Post(CreateCustomerRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Crear cliente";
      s.Description = "Crea un nuevo cliente en el sistema. El cliente se identifica por su número de teléfono, por lo que no se pueden crear clientes con el mismo número.";
      s.ExampleRequest = new CreateCustomerRequest
      {
        FirstName = "María",
        LastName = "López",
        PhoneNumber = "78945612",
        Gender = 2,
        Email = "maria.lopez@ejemplo.com",
        DateOfBirth = new DateTime(1990, 5, 15)
      };
      s.ResponseExamples[201] = new CreateCustomerResponse(
        new CustomerRecord(
          1,
          "CUST-0001",
          "María López",
          "78945612",
          "maria.lopez@ejemplo.com",
          0,
          true
        )
      );
      s.Responses[201] = "Cliente creado exitosamente";
      s.Responses[400] = "Error de validación o cliente ya existe";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administradores o Gerentes)";
    });

    Tags("Customers");

    Description(builder => builder
      .Accepts<CreateCustomerRequest>("application/json")
      .Produces<CreateCustomerResponse>(201, "application/json")
      .ProducesProblem(400));
  }

  public override async Task<Results<Created<CreateCustomerResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateCustomerRequest req, CancellationToken ct)
  {
    var command = new CreateCustomerCommand(
      req.FirstName,
      req.LastName,
      req.PhoneNumber,
      req.Gender,
      req.Email,
      req.DateOfBirth,
      req.Address,
      req.City,
      req.Allergies,
      req.MedicalConditions,
      req.Medications,
      req.PreferredAssistantId,
      User.GetUserId()
    );

    var result = await mediator.Send(command, ct);

    if (!result.IsSuccess)
    {
      return TypedResults.Problem(
        title: "Customer creation failed",
        detail: result.Errors.FirstOrDefault(),
        statusCode: 400
      );
    }

    var response = Map.FromEntity(result.Value);
    return TypedResults.Created($"/api/customers/{result.Value.Id}", response);
  }
}

public sealed class CreateCustomerMapper
  : Mapper<CreateCustomerRequest, CreateCustomerResponse, CustomerDTO>
{
  public override CreateCustomerResponse FromEntity(CustomerDTO e)
    => new(new CustomerRecord(
      e.Id,
      e.CustomerNumber,
      e.FullName,
      e.PhoneNumber,
      e.Email,
      e.LoyaltyPoints,
      e.IsActive
    ));
}