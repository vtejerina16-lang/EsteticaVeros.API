using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate.Handlers;

public class AppointmentRescheduledHandler(ILogger<AppointmentRescheduledHandler> logger)
  : INotificationHandler<AppointmentRescheduledEvent>
{
  public ValueTask Handle(AppointmentRescheduledEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Appointment rescheduled: {AppointmentNumber}",
      domainEvent.Appointment.AppointmentNumber);

    // TODO: Enviar notificación de reprogramación
    // TODO: Actualizar calendario

    return ValueTask.CompletedTask;
  }
}