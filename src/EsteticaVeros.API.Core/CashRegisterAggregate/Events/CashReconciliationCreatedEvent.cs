namespace EsteticaVeros.API.Core.CashRegisterAggregate.Events;

public sealed class CashReconciliationCreatedEvent(CashReconciliation reconciliation) : DomainEventBase
{
  public CashReconciliation Reconciliation { get; } = reconciliation;
}