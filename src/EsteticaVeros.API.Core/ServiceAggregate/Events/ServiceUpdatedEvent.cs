namespace EsteticaVeros.API.Core.ServiceAggregate.Events;

public sealed class ServiceUpdatedEvent(Service service) : DomainEventBase
{
  public Service Service { get; } = service;
}