using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate.Handlers;

public class AppointmentStartedHandler(ILogger<AppointmentStartedHandler> logger)
  : INotificationHandler<AppointmentStartedEvent>
{
  public ValueTask Handle(AppointmentStartedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Appointment started: {AppointmentNumber} at {StartTime}",
      domainEvent.Appointment.AppointmentNumber,
      domainEvent.Appointment.ActualStartTime);

    // TODO: Iniciar contador de tiempo
    // TODO: Actualizar estado en tiempo real

    return ValueTask.CompletedTask;
  }
}