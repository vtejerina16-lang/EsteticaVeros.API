using EsteticaVeros.API.Core.ServiceAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceAggregate.Handlers;

public class ServiceItemsChangedHandler(ILogger<ServiceItemsChangedHandler> logger)
  : INotificationHandler<ServiceItemsChangedEvent>
{
  public ValueTask Handle(ServiceItemsChangedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Service items changed: {ServiceNumber} - Total items: {Count}",
      domainEvent.Service.ServiceNumber,
      domainEvent.Service.ServiceItems.Count);

    return ValueTask.CompletedTask;
  }
}