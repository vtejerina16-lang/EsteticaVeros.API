namespace EsteticaVeros.API.Core.AppointmentAggregate.Events;

public sealed class AppointmentStartedEvent(Appointment appointment) : DomainEventBase
{
  public Appointment Appointment { get; } = appointment;
}