namespace EsteticaVeros.API.Web.Appointments;

public class CreateAppointmentResponse(AppointmentRecord appointment)
{
  public AppointmentRecord Appointment { get; set; } = appointment;
}