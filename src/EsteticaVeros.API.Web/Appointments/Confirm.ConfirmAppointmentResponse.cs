namespace EsteticaVeros.API.Web.Appointments;

public class ConfirmAppointmentResponse(AppointmentRecord appointment)
{
  public AppointmentRecord Appointment { get; set; } = appointment;
}