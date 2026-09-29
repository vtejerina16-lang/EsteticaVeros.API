using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate.Handlers;

public class CashMovementCreatedHandler(ILogger<CashMovementCreatedHandler> logger)
  : INotificationHandler<CashMovementCreatedEvent>
{
  public ValueTask Handle(CashMovementCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Cash movement created: {MovementNumber} - Type: {Type} - Amount: {Amount:C}",
      domainEvent.Movement.MovementNumber,
      domainEvent.Movement.Type.DisplayName,
      domainEvent.Movement.Amount);

    // TODO: Actualizar ExpectedAmount de la sesión

    return ValueTask.CompletedTask;
  }
}