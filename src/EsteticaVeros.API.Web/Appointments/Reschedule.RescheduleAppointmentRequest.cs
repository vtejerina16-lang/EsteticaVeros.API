using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Appointments;

public class RescheduleAppointmentRequest
{
  public const string Route = "/api/appointments/{AppointmentId:int}/reschedule";
  public static string BuildRoute(int appointmentId) => Route.Replace("{AppointmentId:int}", appointmentId.ToString());

  public int AppointmentId { get; set; }

  [Required]
  public DateTime NewAppointmentDate { get; set; }

  [Required]
  public string Reason { get; set; } = string.Empty;
}