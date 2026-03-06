namespace EsteticaVeros.API.Core.AppointmentAggregate.Events;

public sealed class AppointmentScheduledEvent(Appointment appointment) : DomainEventBase
{
  public Appointment Appointment { get; } = appointment;
}