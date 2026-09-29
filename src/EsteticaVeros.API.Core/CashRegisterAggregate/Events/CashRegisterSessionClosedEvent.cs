namespace EsteticaVeros.API.Core.CashRegisterAggregate.Events;

public sealed class CashRegisterSessionClosedEvent(CashRegisterSession session) : DomainEventBase
{
  public CashRegisterSession Session { get; } = session;
}