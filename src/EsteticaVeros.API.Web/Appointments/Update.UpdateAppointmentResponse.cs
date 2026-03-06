namespace EsteticaVeros.API.Web.Appointments;

public class UpdateAppointmentResponse(AppointmentDetailResponse appointment)
{
  public AppointmentDetailResponse Appointment { get; set; } = appointment;
}