namespace EsteticaVeros.API.Web.Appointments;

public class CompleteAppointmentRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}/complete";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }

  public string? Notes { get; set; }
}