using EsteticaVeros.API.Core.ServiceAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceAggregate.Handlers;

public class ServiceUpdatedHandler(ILogger<ServiceUpdatedHandler> logger)
  : INotificationHandler<ServiceUpdatedEvent>
{
  public ValueTask Handle(ServiceUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Service updated: {ServiceNumber} - {Name}",
      domainEvent.Service.ServiceNumber,
      domainEvent.Service.Name);

    return ValueTask.CompletedTask;
  }
}