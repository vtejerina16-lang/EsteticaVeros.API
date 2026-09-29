using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate.Handlers;

public class CashRegisterSessionClosedHandler(ILogger<CashRegisterSessionClosedHandler> logger)
  : INotificationHandler<CashRegisterSessionClosedEvent>
{
  public ValueTask Handle(CashRegisterSessionClosedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Cash register session closed: {SessionNumber} - Expected Amount: {Amount:C}",
      domainEvent.Session.SessionNumber,
      domainEvent.Session.ExpectedAmount);

    return ValueTask.CompletedTask;
  }
}