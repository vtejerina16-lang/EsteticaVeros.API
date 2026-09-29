namespace EsteticaVeros.API.UseCases.Identity.DeactivateUser;

public record DeactivateUserCommand(int UserId) : ICommand<Result>;