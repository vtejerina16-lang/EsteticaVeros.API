namespace EsteticaVeros.API.UseCases.Identity.Login;

/// <summary>
/// Login a user.
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public record LoginCommand(
  string Email,
  string Password,
  bool RememberMe = false
) : ICommand<Result<LoginResult>>;

/// <summary>
/// Response login a user.
/// </summary>
/// <param name="User"></param>
/// <param name="Token"></param>
public record LoginResult(
  UserDTO User,
  string Token,
  DateTime ExpiresAt
);