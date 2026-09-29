namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Events;

public sealed class ServiceCategoryCreatedEvent(ServiceCategory category) : DomainEventBase
{
  public ServiceCategory Category { get; } = category;
}