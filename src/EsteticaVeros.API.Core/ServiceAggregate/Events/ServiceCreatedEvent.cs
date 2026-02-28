namespace EsteticaVeros.API.Core.ServiceAggregate.Events;

public sealed class ServiceCreatedEvent(Service service) : DomainEventBase
{
  public Service Service { get; } = service;
}