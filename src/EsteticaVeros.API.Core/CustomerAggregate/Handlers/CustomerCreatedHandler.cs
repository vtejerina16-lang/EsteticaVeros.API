using EsteticaVeros.API.Core.CustomerAggregate.Events;

namespace EsteticaVeros.API.Core.CustomerAggregate.Handlers;

public class CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
  : INotificationHandler<CustomerCreatedEvent>
{
  public ValueTask Handle(CustomerCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "New customer created: {CustomerNumber} - {FullName} ({PhoneNumber})",
      domainEvent.Customer.CustomerNumber,
      domainEvent.Customer.FullName,
      domainEvent.Customer.PhoneNumber);

    // TODO: Enviar SMS de bienvenida
    // TODO: Notificar a recepción
    // TODO: Agregar a lista de WhatsApp Business

    return ValueTask.CompletedTask;
  }
}