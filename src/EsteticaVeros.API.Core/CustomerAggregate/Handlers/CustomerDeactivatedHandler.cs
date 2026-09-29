using EsteticaVeros.API.Core.CustomerAggregate.Events;

namespace EsteticaVeros.API.Core.CustomerAggregate.Handlers;

public class CustomerDeactivatedHandler(ILogger<CustomerDeactivatedHandler> logger)
  : INotificationHandler<CustomerDeactivatedEvent>
{
  public ValueTask Handle(CustomerDeactivatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Customer deactivated: {CustomerNumber} - {FullName}",
      domainEvent.Customer.CustomerNumber,
      domainEvent.Customer.FullName);

    // TODO: Cancelar citas futuras
    // TODO: Notificar al equipo
    // TODO: Archivar datos

    return ValueTask.CompletedTask;
  }
}