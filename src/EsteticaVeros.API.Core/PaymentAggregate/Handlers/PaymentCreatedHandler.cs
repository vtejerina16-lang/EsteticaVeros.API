using EsteticaVeros.API.Core.PaymentAggregate.Events;

namespace EsteticaVeros.API.Core.PaymentAggregate.Handlers;

public class PaymentCreatedHandler(ILogger<PaymentCreatedHandler> logger)
  : INotificationHandler<PaymentCreatedEvent>
{
  public ValueTask Handle(PaymentCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Payment created: {PaymentNumber} - Amount: {Amount:C}",
      domainEvent.Payment.PaymentNumber,
      domainEvent.Payment.Amount);

    // TODO: Actualizar saldo de sesión de caja
    // TODO: Actualizar estado de pago de la cita
    // TODO: Generar recibo

    return ValueTask.CompletedTask;
  }
}