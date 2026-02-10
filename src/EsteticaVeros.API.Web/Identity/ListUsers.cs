using EsteticaVeros.API.UseCases.Identity;
using EsteticaVeros.API.UseCases.Identity.ListUsers;
using EsteticaVeros.API.Web.Configurations;

namespace EsteticaVeros.API.Web.Identity;

public class ListUsers(IMediator mediator)
  : Endpoint<ListUsersRequest, UsersListResponse, ListUsersMapper>
{
  private readonly IMediator _mediator = mediator;
  public override void Configure()
  {
    Get("/api/users");
    Policies(AuthorizationConfig.RequireAdminRole);

    Summary(s =>
    {
      s.Summary = "Lista los usuarios del sistema";
      s.Description = "Obtiene una lista paginada de usuarios, con opciones de filtrado por estado, rol y término de búsqueda.";
      s.ExampleRequest = new ListUsersRequest {
        Page = 1,
        PerPage = 10,
        IsActive = true,
        SearchTerm = "Juan"
      };

      s.Params["page"] = "1-based page index (default 1)";
      s.Params["per_page"] = "Page size 1–100 (default 10)";
      s.Params["is_active"] = "Filter by active status (optional)";
      s.Params["role"] = "Filter by role name (optional)";
      s.Params["search"] = "Search by name or email (optional)";

      s.Responses[200] = "Lista de usuarios obtenida exitosamente";
      s.Responses[400] = "Solicitud inválida (e.g. parámetros de consulta no válidos)";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo administradores)";
    });

    Tags("Users");

    Description(builder => builder
      .Accepts<ListUsersRequest>()
      .Produces<UsersListResponse>(200, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401)
      .ProducesProblem(403));
  }

  public override async Task HandleAsync(ListUsersRequest req, CancellationToken ct)
  {
    var query = new ListUsersQuery(
      req.Page,
      req.PerPage,
      req.IsActive,
      req.RoleName,
      req.SearchTerm
    );

    var result = await _mediator.Send(query, ct);

    if (!result.IsSuccess)
    {
      await Send.ErrorsAsync(statusCode: 400, ct);
      return;
    }

    var pagedResult = result.Value;

    var response = Map.FromEntity(pagedResult);

    await Send.OkAsync(response, ct);
  }
}

public class ListUsersRequest
{
  [BindFrom("page")]
  public int Page { get; init; } = 1;

  [BindFrom("per_page")]
  public int PerPage { get; init; } = 10;

  [BindFrom("is_active")]
  public bool? IsActive { get; init; }

  [BindFrom("role")]
  public string? RoleName { get; init; }

  [BindFrom("search")]
  public string? SearchTerm { get; init; }
}

public record UsersListResponse(
  IReadOnlyList<UserRecord> Users,
  int Page,
  int PerPage,
  int TotalCount,
  int TotalPages
);

public sealed class ListUsersMapper
  : Mapper<ListUsersRequest, UsersListResponse, UseCases.PagedResult<UserDTO>>
{
  public override UsersListResponse FromEntity(UseCases.PagedResult<UserDTO> e)
  {
    var users = e.Items
      .Select(u => new UserRecord(
        u.Id,
        u.Email,
        u.FirstName,
        u.LastName,
        u.FullName,
        u.PhoneNumber,
        u.Address,
        u.DateOfBirth,
        u.ProfilePictureUrl,
        u.IsActive,
        u.CreatedAt,
        u.LastLoginAt,
        u.RoleName
      ))
      .ToList();

    return new UsersListResponse(users, e.Page, e.PerPage, e.TotalCount, e.TotalPages);
  }
}