namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Events;

public sealed class ServiceCategoryDeactivatedEvent(ServiceCategory category) : DomainEventBase
{
  public ServiceCategory Category { get; } = category;
}