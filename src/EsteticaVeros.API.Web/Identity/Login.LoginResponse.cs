namespace EsteticaVeros.API.Web.Identity;

public record LoginResponse(
  int UserId,
  string Email,
  string FirstName,
  string LastName,
  string Role,
  string Token,
  DateTime ExpiresAt
);