namespace EsteticaVeros.API.Core.CustomerAggregate.Events;

public sealed class CustomerDeactivatedEvent(Customer customer) : DomainEventBase
{
  public Customer Customer { get; } = customer;
}