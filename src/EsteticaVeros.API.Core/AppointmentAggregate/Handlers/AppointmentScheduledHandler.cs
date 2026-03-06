using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate.Handlers;

public class AppointmentScheduledHandler(ILogger<AppointmentScheduledHandler> logger)
  : INotificationHandler<AppointmentScheduledEvent>
{
  public ValueTask Handle(AppointmentScheduledEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Appointment scheduled: {AppointmentNumber} for customer {CustomerId} on {Date}",
      domainEvent.Appointment.AppointmentNumber,
      domainEvent.Appointment.CustomerId,
      domainEvent.Appointment.AppointmentDate);

    // TODO: Enviar confirmación por email/SMS
    // TODO: Crear recordatorio programado

    return ValueTask.CompletedTask;
  }
}