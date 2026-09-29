namespace EsteticaVeros.API.UseCases.CashRegisterSessions.Open;

public record OpenCashRegisterSessionCommand(
  int CashRegisterId,
  decimal InitialAmount,
  int OpenedByUserId
) : ICommand<Result<int>>;