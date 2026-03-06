namespace EsteticaVeros.API.Core.AppointmentAggregate.Events;

public sealed class AppointmentCompletedEvent(Appointment appointment) : DomainEventBase
{
  public Appointment Appointment { get; } = appointment;
}