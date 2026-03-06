namespace EsteticaVeros.API.Web.Appointments;

public class RescheduleAppointmentResponse(AppointmentRecord appointment)
{
  public AppointmentRecord Appointment { get; set; } = appointment;
}