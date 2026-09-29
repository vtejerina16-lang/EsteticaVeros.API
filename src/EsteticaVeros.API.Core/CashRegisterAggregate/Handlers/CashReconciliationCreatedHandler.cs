using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate.Handlers;

public class CashReconciliationCreatedHandler(ILogger<CashReconciliationCreatedHandler> logger)
  : INotificationHandler<CashReconciliationCreatedEvent>
{
  public ValueTask Handle(CashReconciliationCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Cash reconciliation created: {ReconciliationNumber} - Difference: {Difference:C}",
      domainEvent.Reconciliation.ReconciliationNumber,
      domainEvent.Reconciliation.Difference);

    return ValueTask.CompletedTask;
  }
}