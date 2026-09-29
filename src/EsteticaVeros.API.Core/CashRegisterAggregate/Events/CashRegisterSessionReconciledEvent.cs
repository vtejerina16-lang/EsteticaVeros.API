namespace EsteticaVeros.API.Core.CashRegisterAggregate.Events;

public sealed class CashRegisterSessionReconciledEvent(CashRegisterSession session) : DomainEventBase
{
  public CashRegisterSession Session { get; } = session;
}