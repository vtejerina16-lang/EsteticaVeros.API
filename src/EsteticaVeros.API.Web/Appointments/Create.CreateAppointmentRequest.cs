using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Appointments;

public class CreateAppointmentRequest
{
  public const string Route = "/api/appointments";

  [Required]
  public int CustomerId { get; set; }

  [Required]
  public DateTime AppointmentDate { get; set; }

  [Required]
  public int Source { get; set; } // 1=InOffice, 2=WhatsApp, etc.

  [Required]
  public List<int> ServiceIds { get; set; } = new();

  public int? AssignedAssistantId { get; set; }

  public string? Notes { get; set; }

  public string? CustomerNotes { get; set; }

  public string? InternalNotes { get; set; }

  public bool IsRecurring { get; set; } = false;

  public string? RecurrencePattern { get; set; }

  public DateTime? RecurrenceEndDate { get; set; }
}