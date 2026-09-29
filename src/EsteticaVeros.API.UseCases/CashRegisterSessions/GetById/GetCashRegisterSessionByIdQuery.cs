namespace EsteticaVeros.API.UseCases.CashRegisterSessions.GetById;

public record GetCashRegisterSessionByIdQuery(int SessionId) : IQuery<Result<CashRegisterSessionDTO>>;