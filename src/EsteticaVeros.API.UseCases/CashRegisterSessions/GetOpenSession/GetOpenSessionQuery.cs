namespace EsteticaVeros.API.UseCases.CashRegisterSessions.GetOpenSession;

public record GetOpenSessionQuery(int CashRegisterId) : IQuery<Result<CashRegisterSessionDTO?>>;