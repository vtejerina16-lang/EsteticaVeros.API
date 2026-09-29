namespace EsteticaVeros.API.Web.Identity;

public record UserRecord(
  int Id,
  string Email,
  string FirstName,
  string LastName,
  string FullName,
  string? PhoneNumber,
  string? Address,
  DateTime? DateOfBirth,
  string? ProfilePictureUrl,
  bool IsActive,
  DateTime CreatedAt,
  DateTime? LastLoginAt,
  string Role
);