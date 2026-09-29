namespace EsteticaVeros.API.Core.CashRegisterAggregate.Events;

public sealed class CashMovementCreatedEvent(CashMovement movement) : DomainEventBase
{
  public CashMovement Movement { get; } = movement;
}