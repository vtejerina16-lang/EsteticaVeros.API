using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate.Handlers;

public class AppointmentCheckedInHandler(ILogger<AppointmentCheckedInHandler> logger)
  : INotificationHandler<AppointmentCheckedInEvent>
{
  public ValueTask Handle(AppointmentCheckedInEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Appointment checked-in: {AppointmentNumber}",
      domainEvent.Appointment.AppointmentNumber);

    // TODO: Notificar al asistente que el cliente llegó
    // TODO: Preparar sala/equipo

    return ValueTask.CompletedTask;
  }
}