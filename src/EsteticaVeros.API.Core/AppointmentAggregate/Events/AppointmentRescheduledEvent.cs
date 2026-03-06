namespace EsteticaVeros.API.Core.AppointmentAggregate.Events;

public sealed class AppointmentRescheduledEvent(Appointment appointment) : DomainEventBase
{
  public Appointment Appointment { get; } = appointment;
}