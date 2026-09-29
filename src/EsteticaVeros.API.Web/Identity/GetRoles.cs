using EsteticaVeros.API.Core.IdentityAggregate;
using EsteticaVeros.API.Web.Configurations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace EsteticaVeros.API.Web.Identity;

public class GetRoles(RoleManager<ApplicationRole> roleManager)
  : EndpointWithoutRequest<Ok<RolesResponse>>
{
  public override void Configure()
  {
    Get(GetRolesRequest.Route);
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Lista de roles del sistema";
      s.Description = "Obtiene una lista de roles activos en el sistema, incluyendo su nombre y descripción.";
      s.ResponseExamples[200] = new RolesResponse(new List<RoleRecord>
      {
        new(1, "Administrator", "Administrator - Total acceso al sistema"),
        new(2, "Receptionist", "Receptionist - Operaciones de recepción")
      });

      s.Responses[200] = "Lista de roles devuelta exitosamente";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo administradores)";
    });

    Tags("Authentication");

    Description(builder => builder
      .Produces<RolesResponse>(200, "application/json")
      .ProducesProblem(401)
      .ProducesProblem(403));
  }

  public override async Task<Ok<RolesResponse>> ExecuteAsync(CancellationToken ct)
  {
    var roles = await roleManager.Roles
      .Where(r => r.IsActive)
      .OrderBy(r => r.Name)
      .ToListAsync(ct);

    var roleList = roles
      .Select(r => new RoleRecord(r.Id, r.Name!, r.Description ?? ""))
      .ToList();

    return TypedResults.Ok(new RolesResponse(roleList));
  }
}

public static class GetRolesRequest
{
  public const string Route = "/api/auth/roles";
}

public record RolesResponse(IReadOnlyList<RoleRecord> Roles);

public record RoleRecord(
  int Id,
  string Name,
  string Description
);