using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate.Handlers;

public class AppointmentConfirmedHandler(ILogger<AppointmentConfirmedHandler> logger)
  : INotificationHandler<AppointmentConfirmedEvent>
{
  public ValueTask Handle(AppointmentConfirmedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Appointment confirmed: {AppointmentNumber}",
      domainEvent.Appointment.AppointmentNumber);

    // TODO: Enviar confirmación
    // TODO: Actualizar calendario

    return ValueTask.CompletedTask;
  }
}