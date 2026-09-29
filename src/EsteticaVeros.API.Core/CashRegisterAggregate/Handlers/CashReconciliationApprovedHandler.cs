using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate.Handlers;

public class CashReconciliationApprovedHandler(ILogger<CashReconciliationApprovedHandler> logger)
  : INotificationHandler<CashReconciliationApprovedEvent>
{
  public ValueTask Handle(CashReconciliationApprovedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Cash reconciliation approved: {ReconciliationNumber}",
      domainEvent.Reconciliation.ReconciliationNumber);

    // TODO: Actualizar estado de sesión a Reconciled
    // TODO: Enviar notificación al administrador si hay diferencia significativa

    return ValueTask.CompletedTask;
  }
}