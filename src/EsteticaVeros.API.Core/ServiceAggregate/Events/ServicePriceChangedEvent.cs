namespace EsteticaVeros.API.Core.ServiceAggregate.Events;

public sealed class ServicePriceChangedEvent(Service service, decimal newPrice) : DomainEventBase
{
  public Service Service { get; } = service;
  public decimal NewPrice { get; } = newPrice;
}