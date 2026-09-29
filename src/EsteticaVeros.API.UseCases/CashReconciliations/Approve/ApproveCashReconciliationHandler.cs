using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashReconciliations.Approve;

public class ApproveCashReconciliationHandler(
  IRepository<CashReconciliation> reconciliationRepository,
  IRepository<CashRegisterSession> sessionRepository)
  : ICommandHandler<ApproveCashReconciliationCommand, Result>
{
  public async ValueTask<Result> Handle(
    ApproveCashReconciliationCommand command,
    CancellationToken cancellationToken)
  {
    var reconciliation = await reconciliationRepository.GetByIdAsync(command.ReconciliationId, cancellationToken);

    if (reconciliation == null)
    {
      return Result.NotFound("Reconciliation not found");
    }

    try
    {
      reconciliation.Approve(command.ApprovedByUserId, command.Notes);

      // Actualizar sesión a estado Reconciled
      var session = await sessionRepository.GetByIdAsync(reconciliation.CashRegisterSessionId, cancellationToken);
      if (session != null)
      {
        session.RecordReconciliation(reconciliation.CountedAmount, command.Notes);
      }

      await reconciliationRepository.UpdateAsync(reconciliation, cancellationToken);
      await reconciliationRepository.SaveChangesAsync(cancellationToken);

      return Result.Success();
    }
    catch (InvalidOperationException ex)
    {
      return Result.Error(ex.Message);
    }
  }
}