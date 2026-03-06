using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate.Handlers;

public class AppointmentNoShowHandler(ILogger<AppointmentNoShowHandler> logger)
  : INotificationHandler<AppointmentNoShowEvent>
{
  public ValueTask Handle(AppointmentNoShowEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Appointment no-show: {AppointmentNumber}",
      domainEvent.Appointment.AppointmentNumber);

    // TODO: Registrar penalización
    // TODO: Incrementar contador de no-shows del cliente
    // TODO: Notificar al equipo

    return ValueTask.CompletedTask;
  }
}