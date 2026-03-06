namespace EsteticaVeros.API.Web.Appointments;

public class StartAppointmentRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}/start";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }
}