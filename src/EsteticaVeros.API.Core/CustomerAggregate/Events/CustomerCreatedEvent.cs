namespace EsteticaVeros.API.Core.CustomerAggregate.Events;

public sealed class CustomerCreatedEvent(Customer customer) : DomainEventBase
{
  public Customer Customer { get; } = customer;
}