namespace EsteticaVeros.API.UseCases.CashRegisterSessions.Close;

public record CloseCashRegisterSessionCommand(
  int SessionId,
  int ClosedByUserId,
  string? Notes = null
) : ICommand<Result>;