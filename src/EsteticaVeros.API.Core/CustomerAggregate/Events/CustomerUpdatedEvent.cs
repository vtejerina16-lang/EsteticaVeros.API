namespace EsteticaVeros.API.Core.CustomerAggregate.Events;

public sealed class CustomerUpdatedEvent(Customer customer) : DomainEventBase
{
  public Customer Customer { get; } = customer;
}