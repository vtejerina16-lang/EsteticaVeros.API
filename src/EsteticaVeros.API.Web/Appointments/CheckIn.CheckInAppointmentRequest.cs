namespace EsteticaVeros.API.Web.Appointments;

public class CheckInAppointmentRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}/check-in";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }
}