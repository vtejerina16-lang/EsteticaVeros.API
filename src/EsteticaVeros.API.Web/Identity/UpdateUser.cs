using EsteticaVeros.API.UseCases.Identity;
using EsteticaVeros.API.UseCases.Identity.UpdateUser;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Identity;

public class UpdateUser(IMediator mediator)
  : Endpoint<UpdateUserRequest,
            Results<Ok<UpdateUserResponse>,
              NotFound,
              ProblemHttpResult>,
            UpdateUserMapper>
{
  public override void Configure()
  {
    Put(UpdateUserRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Actualiza un usuario existente";
      s.Description = "Actualiza los detalles de un usuario, incluyendo su rol y estado activo. Solo los administradores pueden realizar esta acción.";
      s.ExampleRequest = new UpdateUserRequest
      {
        UserId = 2,
        FirstName = "Juan",
        LastName = "Pérez",
        PhoneNumber = "12345678",
        RoleName = "Receptionist",
        IsActive = true
      };
      s.ResponseExamples[200] = new UpdateUserResponse(
        new UserRecord(
          2,
          "juan@ejemplo.com",
          "Juan",
          "Pérez",
          "Juan Pérez",
          "12345678",
          null,
          null,
          null,
          true,
          DateTime.UtcNow,
          null,
          "Receptionist"
        )
      );

      s.Responses[200] = "Usuario actualizado exitosamente";
      s.Responses[400] = "Solicitud inválida (datos incorrectos o faltantes)";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo administradores)";
      s.Responses[404] = "Usuario con ID especificado no encontrado";
    });

    Tags("Users");

    Description(builder => builder
      .Accepts<UpdateUserRequest>("application/json")
      .Produces<UpdateUserResponse>(200, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401)
      .ProducesProblem(403)
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<UpdateUserResponse>, NotFound, ProblemHttpResult>> ExecuteAsync(
    UpdateUserRequest req,
    CancellationToken ct)
  {
    var command = new UpdateUserCommand(
      req.UserId,
      req.FirstName,
      req.LastName,
      req.PhoneNumber,
      req.Address,
      req.DateOfBirth,
      req.RoleName,
      req.IsActive
    );

    var result = await mediator.Send(command, ct);

    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class UpdateUserMapper
  : Mapper<UpdateUserRequest, UpdateUserResponse, UserDTO>
{
  public override UpdateUserResponse FromEntity(UserDTO e)
    => new(new UserRecord(
      e.Id,
      e.Email,
      e.FirstName,
      e.LastName,
      e.FullName,
      e.PhoneNumber,
      e.Address,
      e.DateOfBirth,
      e.ProfilePictureUrl,
      e.IsActive,
      e.CreatedAt,
      e.LastLoginAt,
      e.RoleName
    ));
}