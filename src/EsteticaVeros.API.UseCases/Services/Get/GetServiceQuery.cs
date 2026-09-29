namespace EsteticaVeros.API.UseCases.Services.Get;

public record GetServiceQuery(int ServiceId) : IQuery<Result<ServiceDTO>>;