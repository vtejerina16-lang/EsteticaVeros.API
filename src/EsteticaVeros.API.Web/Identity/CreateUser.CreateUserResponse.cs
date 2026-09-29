namespace EsteticaVeros.API.Web.Identity;

public record CreateUserResponse(
  int UserId,
  string Email,
  string FirstName,
  string LastName,
  string Role
);