using EsteticaVeros.API.UseCases.Identity.Login;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Identity;

public class Login(IMediator mediator)
  : Endpoint<LoginRequest,
             Results<Ok<LoginResponse>,
                     UnauthorizedHttpResult,
                     ForbidHttpResult,
                     ProblemHttpResult>>
{
  private readonly IMediator _mediator = mediator;
  public override void Configure()
  {
    Post(LoginRequest.Route);
    AllowAnonymous();
    
    Summary(s =>
    {
      s.Summary = "Entrar al sistema";
      s.Description = "Autentica a un usuario y devuelve un token JWT para acceder a los recursos protegidos de la API. El token incluye información del usuario y su rol, y tiene una fecha de expiración.";
      s.ExampleRequest = new LoginRequest 
      { 
        Email = "admin@esteticaveros.com", 
        Password = "Admin123!", 
        RememberMe = false 
      };
      s.ResponseExamples[200] = new LoginResponse(
        1,
        "admin@esteticaveros.com",
        "Admin",
        "Sistema",
        "Admin",
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        DateTime.UtcNow.AddHours(8)
      );

      s.Responses[200] = "Inicio de sesión exitoso, devuelve un token JWT con la información del usuario";
      s.Responses[401] = "Credenciales inválidas";
      s.Responses[403] = "Cuenta inactiva o bloqueada";
      s.Responses[400] = "Error de validación u otro fallo";
    });

    Tags("Authentication");

    Description(builder => builder
      .Accepts<LoginRequest>("application/json")
      .Produces<LoginResponse>(200, "application/json")
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(400));
  }

  public override async Task<Results<Ok<LoginResponse>, UnauthorizedHttpResult, ForbidHttpResult, ProblemHttpResult>>
    ExecuteAsync(LoginRequest req, CancellationToken ct)
  {
    var command = new LoginCommand(req.Email, req.Password, req.RememberMe);
    var result = await _mediator.Send(command, ct);

    if (result.IsSuccess)
    {
      var loginResult = result.Value;
      var response = new LoginResponse(
        loginResult.User.Id,
        loginResult.User.Email,
        loginResult.User.FirstName,
        loginResult.User.LastName,
        loginResult.User.RoleName,
        loginResult.Token,
        loginResult.ExpiresAt
      );
      return TypedResults.Ok(response);
    }

    if (result.Status == ResultStatus.Unauthorized)
    {
      return TypedResults.Unauthorized();
    }

    if (result.Status == ResultStatus.Forbidden)
    {
      return TypedResults.Forbid();
    }

    return TypedResults.Problem(
      title: "Login failed",
      detail: result.Errors.FirstOrDefault(),
      statusCode: 400
    );
  }
}