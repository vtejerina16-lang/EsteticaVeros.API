namespace EsteticaVeros.API.Web.Appointments;

public class CancelAppointmentResponse(AppointmentDetailResponse appointment)
{
  public AppointmentDetailResponse Appointment { get; set; } = appointment;
}