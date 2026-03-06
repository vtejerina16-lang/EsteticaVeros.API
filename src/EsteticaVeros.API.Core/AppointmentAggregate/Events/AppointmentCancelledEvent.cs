namespace EsteticaVeros.API.Core.AppointmentAggregate.Events;

public sealed class AppointmentCancelledEvent(Appointment appointment, string reason) : DomainEventBase
{
  public Appointment Appointment { get; } = appointment;
  public string Reason { get; } = reason;
}