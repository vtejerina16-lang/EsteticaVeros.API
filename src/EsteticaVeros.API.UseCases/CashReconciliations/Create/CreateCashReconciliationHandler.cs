using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.CashReconciliations.Create;

public class CreateCashReconciliationHandler(
  IRepository<CashReconciliation> reconciliationRepository,
  IReadRepository<CashRegisterSession> sessionRepository)
  : ICommandHandler<CreateCashReconciliationCommand, Result<int>>
{
  public async ValueTask<Result<int>> Handle(
    CreateCashReconciliationCommand command,
    CancellationToken cancellationToken)
  {
    // Validar que la sesión existe
    var sessionSpec = new CashRegisterSessionByIdSpec(command.CashRegisterSessionId);
    var session = await sessionRepository.FirstOrDefaultAsync(sessionSpec, cancellationToken);

    if (session == null)
    {
      return Result.NotFound("Cash register session not found");
    }

    // Validar que la sesión está cerrada
    if (session.Status != SessionStatus.Closed)
    {
      return Result.Error("Session must be closed before reconciliation");
    }

    // Validar que no existe un arqueo pendiente para esta sesión
    var existingReconciliations = await reconciliationRepository.ListAsync(cancellationToken);
    var hasPendingReconciliation = existingReconciliations.Any(r => 
      r.CashRegisterSessionId == command.CashRegisterSessionId && 
      r.Status == ReconciliationStatus.Pending);

    if (hasPendingReconciliation)
    {
      return Result.Error("There is already a pending reconciliation for this session");
    }

    // Generar número de arqueo
    var year = DateTime.UtcNow.Year;
    var reconciliationsThisYear = existingReconciliations.Count(r => r.CreatedAt.Year == year);
    var reconciliationNumber = ReconciliationNumber.Generate(year, reconciliationsThisYear + 1);

    // Crear arqueo
    var reconciliation = new CashReconciliation(
      reconciliationNumber,
      command.CashRegisterSessionId,
      session.ExpectedAmount,
      command.CountedAmount,
      command.ReconciledByUserId,
      command.DenominationDetails,
      command.Notes
    );

    var created = await reconciliationRepository.AddAsync(reconciliation, cancellationToken);
    await reconciliationRepository.SaveChangesAsync(cancellationToken);

    return Result.Success(created.Id);
  }
}