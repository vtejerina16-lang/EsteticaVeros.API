using EsteticaVeros.API.Core.CustomerAggregate.Events;

namespace EsteticaVeros.API.Core.CustomerAggregate.Handlers;

public class CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
  : INotificationHandler<CustomerUpdatedEvent>
{
  public ValueTask Handle(CustomerUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Customer updated: {CustomerNumber} - {FullName}",
      domainEvent.Customer.CustomerNumber,
      domainEvent.Customer.FullName);

    // TODO: Sincronizar con sistemas externos
    // TODO: Notificar cambios importantes

    return ValueTask.CompletedTask;
  }
}