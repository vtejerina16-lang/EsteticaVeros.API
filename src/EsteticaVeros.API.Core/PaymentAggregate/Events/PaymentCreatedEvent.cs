namespace EsteticaVeros.API.Core.PaymentAggregate.Events;

public sealed class PaymentCreatedEvent(Payment payment) : DomainEventBase
{
  public Payment Payment { get; } = payment;
}