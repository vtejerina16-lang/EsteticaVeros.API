namespace EsteticaVeros.API.UseCases.CashReconciliations.Reject;

public record RejectCashReconciliationCommand(
  int ReconciliationId,
  int RejectedByUserId,
  string Reason
) : ICommand<Result>;