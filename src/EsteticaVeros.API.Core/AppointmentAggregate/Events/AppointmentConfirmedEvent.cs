namespace EsteticaVeros.API.Core.AppointmentAggregate.Events;

public sealed class AppointmentConfirmedEvent(Appointment appointment) : DomainEventBase
{
  public Appointment Appointment { get; } = appointment;
}