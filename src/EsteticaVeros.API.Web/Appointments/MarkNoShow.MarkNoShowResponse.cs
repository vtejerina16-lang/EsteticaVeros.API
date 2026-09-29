namespace EsteticaVeros.API.Web.Appointments;

public class MarkNoShowResponse(AppointmentRecord appointment)
{
  public AppointmentRecord Appointment { get; set; } = appointment;
}