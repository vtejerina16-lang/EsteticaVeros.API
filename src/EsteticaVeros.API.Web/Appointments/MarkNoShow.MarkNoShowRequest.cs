namespace EsteticaVeros.API.Web.Appointments;

public class MarkNoShowRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}/no-show";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }

  public string? Reason { get; set; }
}