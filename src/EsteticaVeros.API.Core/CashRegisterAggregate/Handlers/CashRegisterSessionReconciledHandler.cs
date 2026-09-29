using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate.Handlers;

public class CashRegisterSessionReconciledHandler(ILogger<CashRegisterSessionReconciledHandler> logger)
  : INotificationHandler<CashRegisterSessionReconciledEvent>
{
  public ValueTask Handle(CashRegisterSessionReconciledEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Cash register session reconciled: {SessionNumber} - Difference: {Difference:C}",
      domainEvent.Session.SessionNumber,
      domainEvent.Session.Difference);

    return ValueTask.CompletedTask;
  }
}