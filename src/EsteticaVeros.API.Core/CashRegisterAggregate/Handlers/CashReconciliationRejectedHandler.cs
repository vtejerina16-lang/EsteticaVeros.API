using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate.Handlers;

public class CashReconciliationRejectedHandler(ILogger<CashReconciliationRejectedHandler> logger)
  : INotificationHandler<CashReconciliationRejectedEvent>
{
  public ValueTask Handle(CashReconciliationRejectedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Cash reconciliation rejected: {ReconciliationNumber} - Reason: {Notes}",
      domainEvent.Reconciliation.ReconciliationNumber,
      domainEvent.Reconciliation.Notes);

    // TODO: Notificar al usuario que hizo el arqueo
    // TODO: Crear tarea de corrección

    return ValueTask.CompletedTask;
  }
}