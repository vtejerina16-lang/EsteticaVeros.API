namespace EsteticaVeros.API.Core.CashRegisterAggregate.Events;

public sealed class CashReconciliationRejectedEvent(CashReconciliation reconciliation) : DomainEventBase
{
  public CashReconciliation Reconciliation { get; } = reconciliation;
}