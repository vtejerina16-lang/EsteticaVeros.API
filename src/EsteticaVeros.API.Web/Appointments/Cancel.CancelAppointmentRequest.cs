using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Appointments;

public class CancelAppointmentRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}/cancel";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }

  [Required]
  public string CancellationReason { get; set; } = string.Empty;
}