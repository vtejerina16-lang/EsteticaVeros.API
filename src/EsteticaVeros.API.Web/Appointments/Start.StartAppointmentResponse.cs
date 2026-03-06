namespace EsteticaVeros.API.Web.Appointments;

public class StartAppointmentResponse(AppointmentRecord appointment)
{
  public AppointmentRecord Appointment { get; set; } = appointment;
}