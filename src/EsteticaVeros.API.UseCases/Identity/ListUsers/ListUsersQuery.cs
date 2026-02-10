namespace EsteticaVeros.API.UseCases.Identity.ListUsers;

public record ListUsersQuery(
  int Page = 1,
  int PerPage = 10,
  bool? IsActive = null,
  string? RoleName = null,
  string? SearchTerm = null
) : IQuery<Result<PagedResult<UserDTO>>>;