namespace EsteticaVeros.API.Web.Appointments;

public record GetAppointmentByIdRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }
}