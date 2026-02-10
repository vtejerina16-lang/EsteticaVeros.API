namespace EsteticaVeros.API.UseCases.Identity.UpdateUser;

public record UpdateUserCommand(
  int UserId,
  string FirstName,
  string LastName,
  string? PhoneNumber,
  string? Address,
  DateTime? DateOfBirth,
  string RoleName,
  bool IsActive
) : ICommand<Result<UserDTO>>;