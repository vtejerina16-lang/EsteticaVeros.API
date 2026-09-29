namespace EsteticaVeros.API.Core.ServiceAggregate.Events;

public sealed class ServiceDeactivatedEvent(Service service) : DomainEventBase
{
  public Service Service { get; } = service;
}