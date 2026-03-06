using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Appointments;

public class GetAvailableSlotsRequest
{
  public const string Route = "/api/appointments/available-slots";

  [Required]
  [BindFrom("date")]
  public DateTime Date { get; init; }

  [Required]
  [BindFrom("estimated_duration")]
  public int EstimatedDuration { get; init; }

  [BindFrom("assigned_assistant_id")]
  public int? AssignedAssistantId { get; init; }
}