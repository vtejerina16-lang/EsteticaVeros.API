namespace EsteticaVeros.API.UseCases.CashReconciliations.Approve;

public record ApproveCashReconciliationCommand(
  int ReconciliationId,
  int ApprovedByUserId,
  string? Notes = null
) : ICommand<Result>;