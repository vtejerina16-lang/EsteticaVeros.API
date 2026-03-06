namespace EsteticaVeros.API.Core.AppointmentAggregate.Events;

public sealed class AppointmentCheckedInEvent(Appointment appointment) : DomainEventBase
{
  public Appointment Appointment { get; } = appointment;
}