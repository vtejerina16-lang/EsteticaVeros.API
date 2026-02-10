namespace EsteticaVeros.API.UseCases.Identity.GetUser;

public record GetUserQuery(int UserId) : IQuery<Result<UserDTO>>;