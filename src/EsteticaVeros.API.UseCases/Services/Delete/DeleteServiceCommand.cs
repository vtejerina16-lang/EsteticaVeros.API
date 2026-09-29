namespace EsteticaVeros.API.UseCases.Services.Delete;

public record DeleteServiceCommand(int ServiceId) : ICommand<Result>;