namespace EsteticaVeros.API.UseCases.CashReconciliations.Create;

public record CreateCashReconciliationCommand(
  int CashRegisterSessionId,
  decimal CountedAmount,
  int ReconciledByUserId,
  string? DenominationDetails = null,
  string? Notes = null
) : ICommand<Result<int>>;