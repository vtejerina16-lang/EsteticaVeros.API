using EsteticaVeros.API.Core.PaymentAggregate.Events;

namespace EsteticaVeros.API.Core.PaymentAggregate.Handlers;

public class PaymentRefundedHandler(ILogger<PaymentRefundedHandler> logger)
  : INotificationHandler<PaymentRefundedEvent>
{
  public ValueTask Handle(PaymentRefundedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Payment refunded: {PaymentNumber}",
      domainEvent.Payment.PaymentNumber);

    // TODO: Registrar movimiento negativo en caja
    // TODO: Actualizar estado de la cita

    return ValueTask.CompletedTask;
  }
}