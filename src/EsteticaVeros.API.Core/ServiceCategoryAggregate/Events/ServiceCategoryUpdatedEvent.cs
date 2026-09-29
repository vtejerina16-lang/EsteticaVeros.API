namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Events;

public sealed class ServiceCategoryUpdatedEvent(ServiceCategory category) : DomainEventBase
{
  public ServiceCategory Category { get; } = category;
}