using EsteticaVeros.API.UseCases.Identity.CreateUser;
using EsteticaVeros.API.Web.Configurations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Identity;

public class CreateUser(IMediator mediator)
  : Endpoint<CreateUserRequest,
             Results<Created<CreateUserResponse>,
                     ValidationProblem,
                     ProblemHttpResult>>
{
  private readonly IMediator _mediator = mediator;
  public override void Configure()
  {
    Post(CreateUserRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Crear un nuevo usuario";
      s.Description = "Crea un nuevo usuario con el rol especificado. El email debe ser único y el password debe cumplir con los requisitos de seguridad.";
      s.ExampleRequest = new CreateUserRequest
      {
        Email = "juan.perez@ejemplo.com",
        Password = "Password123!",
        FirstName = "Juan",
        LastName = "Pérez",
        PhoneNumber = "12345678"
      };
      s.ResponseExamples[201] = new CreateUserResponse(
        1,
        "juan.perez@ejemplo.com",
        "Juan",
        "Pérez",
        "Receptionist"
      );

      s.Responses[201] = "Usuario creado exitosamente";
      s.Responses[400] = "Error de validación o usuario ya existe";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administradores)";
    });

    Tags("Users");

    Description(builder => builder
      .Accepts<CreateUserRequest>("application/json")
      .Produces<CreateUserResponse>(201, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401)
      .ProducesProblem(403));
  }

  public override async Task<Results<Created<CreateUserResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateUserRequest req, CancellationToken ct)
  {
    var command = new CreateUserCommand(
      req.Email,
      req.Password,
      req.FirstName,
      req.LastName,
      req.PhoneNumber,
      req.RoleName
    );

    var result = await _mediator.Send(command, ct);

    if (result.IsSuccess)
    {
      var user = result.Value;
      var response = new CreateUserResponse(
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        user.RoleName
      );

      return TypedResults.Created($"/api/auth/users/{user.Id}", response);
    }

    return TypedResults.Problem(
      title: "User creation failed",
      detail: result.Errors.FirstOrDefault(),
      statusCode: 400
    );
  }
}