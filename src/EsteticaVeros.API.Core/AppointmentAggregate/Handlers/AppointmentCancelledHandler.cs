using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate.Handlers;

public class AppointmentCancelledHandler(ILogger<AppointmentCancelledHandler> logger)
  : INotificationHandler<AppointmentCancelledEvent>
{
  public ValueTask Handle(AppointmentCancelledEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Appointment cancelled: {AppointmentNumber} - Reason: {Reason}",
      domainEvent.Appointment.AppointmentNumber,
      domainEvent.Reason);

    // TODO: Enviar notificación de cancelación
    // TODO: Procesar reembolso si aplica
    // TODO: Liberar horario

    return ValueTask.CompletedTask;
  }
}