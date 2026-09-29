namespace EsteticaVeros.API.UseCases.Identity;

public record UserDTO(
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
  string RoleName
);