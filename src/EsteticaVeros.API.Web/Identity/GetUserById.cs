using EsteticaVeros.API.Core.IdentityAggregate;
using EsteticaVeros.API.UseCases.Identity;
using EsteticaVeros.API.UseCases.Identity.GetUser;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Identity;

public class GetUserById(IMediator mediator)
  : Endpoint<GetUserByIdRequest,
             Results<Ok<UserRecord>,
                     NotFound,
                     ProblemHttpResult>,
             GetUserByIdMapper>
{
  private readonly IMediator _mediator = mediator;
  public override void Configure()
  {
    Get(GetUserByIdRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Consigue un usuario por su ID";
      s.Description = "Retorna los detalles de un usuario específico utilizando su identificador único. Incluye información como correo electrónico, nombre completo, número de teléfono, dirección y estado de actividad.";
      s.ExampleRequest = new GetUserByIdRequest { UserId = 1 };
      s.ResponseExamples[200] = new UserRecord(
        1,
        "admin@esteticaveros.com",
        "Admin",
        "Sistema",
        "Admin Sistema",
        null,
        null,
        null,
        null,
        true,
        DateTime.UtcNow,
        null,
        "Admin"
      );

      s.Responses[200] = "Usuario encontrado y retornado exitosamente";
      s.Responses[400] = "Solicitud inválida (e.g. parámetros de consulta no válidos)";
      s.Responses[401] = "No autorizado (se requiere autenticación)";
      s.Responses[403] = "No autorizado (solo administradores)";
      s.Responses[404] = "Usuario con el ID especificado no encontrado";
    });

    Tags("Users");

    Description(builder => builder
      .Accepts<GetUserByIdRequest>()
      .Produces<UserRecord>(200, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<UserRecord>, NotFound, ProblemHttpResult>> ExecuteAsync(
    GetUserByIdRequest req,
    CancellationToken ct)
  {
    var query = new GetUserQuery(req.UserId);
    var result = await _mediator.Send(query, ct);

    if (result.IsSuccess)
    {
      var user = result.Value;
      var response = new UserRecord(
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        user.FullName,
        user.PhoneNumber,
        user.Address,
        user.DateOfBirth,
        user.ProfilePictureUrl,
        user.IsActive,
        user.CreatedAt,
        user.LastLoginAt,
        user.RoleName
      );

      return TypedResults.Ok(response);
    }

    return TypedResults.NotFound();
  }
}

public sealed class GetUserByIdMapper
  : Mapper<GetUserByIdRequest, UserRecord, UserDTO>
{
  public override UserRecord FromEntity(UserDTO e)
    => new(e.Id, e.Email, e.FirstName, e.LastName, e.FullName, e.PhoneNumber, e.Address, e.DateOfBirth, e.ProfilePictureUrl, e.IsActive, e.CreatedAt, e.LastLoginAt, e.RoleName);
}