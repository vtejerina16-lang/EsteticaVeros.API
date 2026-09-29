using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashReconciliations.Reject;

public class RejectCashReconciliationHandler(IRepository<CashReconciliation> repository)
  : ICommandHandler<RejectCashReconciliationCommand, Result>
{
  public async ValueTask<Result> Handle(
    RejectCashReconciliationCommand command,
    CancellationToken cancellationToken)
  {
    var reconciliation = await repository.GetByIdAsync(command.ReconciliationId, cancellationToken);

    if (reconciliation == null)
    {
      return Result.NotFound("Reconciliation not found");
    }

    try
    {
      reconciliation.Reject(command.RejectedByUserId, command.Reason);

      await repository.UpdateAsync(reconciliation, cancellationToken);
      await repository.SaveChangesAsync(cancellationToken);

      return Result.Success();
    }
    catch (InvalidOperationException ex)
    {
      return Result.Error(ex.Message);
    }
  }
}