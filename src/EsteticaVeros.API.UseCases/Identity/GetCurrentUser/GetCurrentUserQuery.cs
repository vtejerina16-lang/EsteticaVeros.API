namespace EsteticaVeros.API.UseCases.Identity.GetCurrentUser;

public record GetCurrentUserQuery(int UserId) : IQuery<Result<UserDTO>>;