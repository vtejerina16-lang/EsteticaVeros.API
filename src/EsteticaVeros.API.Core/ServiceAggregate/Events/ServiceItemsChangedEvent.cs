namespace EsteticaVeros.API.Core.ServiceAggregate.Events;

public sealed class ServiceItemsChangedEvent(Service service) : DomainEventBase
{
  public Service Service { get; } = service;
}