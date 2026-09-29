using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate.Handlers;

public class CashRegisterSessionOpenedHandler(ILogger<CashRegisterSessionOpenedHandler> logger)
  : INotificationHandler<CashRegisterSessionOpenedEvent>
{
  public ValueTask Handle(CashRegisterSessionOpenedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Cash register session opened: {SessionNumber} - Initial Amount: {Amount:C}",
      domainEvent.Session.SessionNumber,
      domainEvent.Session.InitialAmount);

    return ValueTask.CompletedTask;
  }
}