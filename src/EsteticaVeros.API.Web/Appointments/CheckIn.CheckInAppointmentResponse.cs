namespace EsteticaVeros.API.Web.Appointments;

public class CheckInAppointmentResponse(AppointmentRecord appointment)
{
  public AppointmentRecord Appointment { get; set; } = appointment;
}