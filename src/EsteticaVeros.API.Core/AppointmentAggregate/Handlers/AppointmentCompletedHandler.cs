using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate.Handlers;

public class AppointmentCompletedHandler(ILogger<AppointmentCompletedHandler> logger)
  : INotificationHandler<AppointmentCompletedEvent>
{
  public ValueTask Handle(AppointmentCompletedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Appointment completed: {AppointmentNumber} - Duration: {Duration} minutes",
      domainEvent.Appointment.AppointmentNumber,
      domainEvent.Appointment.ActualDuration);

    // TODO: Actualizar estadísticas del cliente
    // TODO: Agregar puntos de lealtad
    // TODO: Enviar encuesta de satisfacción

    return ValueTask.CompletedTask;
  }
}