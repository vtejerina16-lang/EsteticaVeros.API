namespace EsteticaVeros.API.Core.PaymentAggregate.Events;

public sealed class PaymentRefundedEvent(Payment payment) : DomainEventBase
{
  public Payment Payment { get; } = payment;
}