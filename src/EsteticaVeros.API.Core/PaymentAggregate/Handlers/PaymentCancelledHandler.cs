using EsteticaVeros.API.Core.PaymentAggregate.Events;

namespace EsteticaVeros.API.Core.PaymentAggregate.Handlers;

public class PaymentCancelledHandler(ILogger<PaymentCancelledHandler> logger)
  : INotificationHandler<PaymentCancelledEvent>
{
  public ValueTask Handle(PaymentCancelledEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Payment cancelled: {PaymentNumber}",
      domainEvent.Payment.PaymentNumber);

    return ValueTask.CompletedTask;
  }
}