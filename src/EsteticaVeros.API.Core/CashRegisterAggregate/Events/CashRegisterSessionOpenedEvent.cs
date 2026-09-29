namespace EsteticaVeros.API.Core.CashRegisterAggregate.Events;

public sealed class CashRegisterSessionOpenedEvent(CashRegisterSession session) : DomainEventBase
{
  public CashRegisterSession Session { get; } = session;
}