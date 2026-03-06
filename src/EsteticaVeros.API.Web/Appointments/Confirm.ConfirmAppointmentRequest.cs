namespace EsteticaVeros.API.Web.Appointments;

public class ConfirmAppointmentRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}/confirm";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }
}