using EsteticaVeros.API.Core.ServiceAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceAggregate.Handlers;

public class ServicePriceChangedHandler(ILogger<ServicePriceChangedHandler> logger)
  : INotificationHandler<ServicePriceChangedEvent>
{
  public ValueTask Handle(ServicePriceChangedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Service price changed: {ServiceNumber} - New price: {Price:C}",
      domainEvent.Service.ServiceNumber,
      domainEvent.NewPrice);

    // TODO: Notificar a clientes con citas programadas
    // TODO: Actualizar precios en sistema de facturación

    return ValueTask.CompletedTask;
  }
}