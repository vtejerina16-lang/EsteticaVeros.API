using EsteticaVeros.API.Core.ServiceAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceAggregate.Handlers;

public class ServiceCreatedHandler(ILogger<ServiceCreatedHandler> logger)
  : INotificationHandler<ServiceCreatedEvent>
{
  public ValueTask Handle(ServiceCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "New service created: {ServiceNumber} - {Name} (Type: {Type})",
      domainEvent.Service.ServiceNumber,
      domainEvent.Service.Name,
      domainEvent.Service.ServiceType.DisplayName);

    return ValueTask.CompletedTask;
  }
}