using EsteticaVeros.API.Core.ServiceAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceAggregate.Handlers;

public class ServiceDeactivatedHandler(ILogger<ServiceDeactivatedHandler> logger)
  : INotificationHandler<ServiceDeactivatedEvent>
{
  public ValueTask Handle(ServiceDeactivatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Service deactivated: {ServiceNumber} - {Name}",
      domainEvent.Service.ServiceNumber,
      domainEvent.Service.Name);

    // TODO: Cancelar citas futuras con este servicio
    // TODO: Notificar al equipo

    return ValueTask.CompletedTask;
  }
}