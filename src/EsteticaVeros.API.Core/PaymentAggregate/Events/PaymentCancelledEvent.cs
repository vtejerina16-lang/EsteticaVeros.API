namespace EsteticaVeros.API.Core.PaymentAggregate.Events;

public sealed class PaymentCancelledEvent(Payment payment) : DomainEventBase
{
  public Payment Payment { get; } = payment;
}