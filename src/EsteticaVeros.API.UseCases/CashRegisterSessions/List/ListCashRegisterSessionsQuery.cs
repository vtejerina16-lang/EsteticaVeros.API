namespace EsteticaVeros.API.UseCases.CashRegisterSessions.List;

public record ListCashRegisterSessionsQuery(
  int? CashRegisterId = null,
  DateTime? StartDate = null,
  DateTime? EndDate = null,
  string? Status = null
) : IQuery<Result<IEnumerable<CashRegisterSessionDTO>>>;