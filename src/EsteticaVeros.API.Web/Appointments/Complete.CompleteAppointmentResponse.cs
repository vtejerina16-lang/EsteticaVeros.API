namespace EsteticaVeros.API.Web.Appointments;

public class CompleteAppointmentResponse(AppointmentRecord appointment)
{
  public AppointmentRecord Appointment { get; set; } = appointment;
}