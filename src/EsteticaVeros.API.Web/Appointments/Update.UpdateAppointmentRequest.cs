namespace EsteticaVeros.API.Web.Appointments;

public class UpdateAppointmentRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }

  public DateTime? AppointmentDate { get; set; }

  public List<int>? ServiceIds { get; set; }

  public int? AssignedAssistantId { get; set; }

  public string? Notes { get; set; }

  public string? CustomerNotes { get; set; }

  public string? InternalNotes { get; set; }
}