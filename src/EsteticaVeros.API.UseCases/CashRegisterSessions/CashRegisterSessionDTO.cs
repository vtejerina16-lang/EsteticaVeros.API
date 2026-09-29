namespace EsteticaVeros.API.UseCases.CashRegisterSessions;

public record CashRegisterSessionDTO(
  int Id,
  string SessionNumber,
  int CashRegisterId,
  string CashRegisterName,
  int OpenedByUserId,
  int? ClosedByUserId,
  DateTime OpenedAt,
  DateTime? ClosedAt,
  decimal InitialAmount,
  decimal ExpectedAmount,
  decimal? ActualAmount,
  decimal? Difference,
  string Status,
  string StatusName,
  string? Notes
);