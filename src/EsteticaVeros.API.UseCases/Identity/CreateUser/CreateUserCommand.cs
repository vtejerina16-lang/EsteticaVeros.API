namespace EsteticaVeros.API.UseCases.Identity.CreateUser;

public record CreateUserCommand(
  string Email,
  string Password,
  string FirstName,
  string LastName,
  string? PhoneNumber = null,
  string RoleName = "Receptionist"
) : ICommand<Result<UserDTO>>;