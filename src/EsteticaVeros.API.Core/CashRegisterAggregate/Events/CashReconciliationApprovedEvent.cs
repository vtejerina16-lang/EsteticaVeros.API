namespace EsteticaVeros.API.Core.CashRegisterAggregate.Events;

public sealed class CashReconciliationApprovedEvent(CashReconciliation reconciliation) : DomainEventBase
{
  public CashReconciliation Reconciliation { get; } = reconciliation;
}