namespace EsteticaVeros.API.Core.AppointmentAggregate.Events;

public sealed class AppointmentNoShowEvent(Appointment appointment) : DomainEventBase
{
  public Appointment Appointment { get; } = appointment;
}