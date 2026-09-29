namespace EsteticaVeros.API.Web.Appointments;

public class ListAppointmentsRequest
{
  [BindFrom("start_date")]
  public DateTime? StartDate { get; init; }

  [BindFrom("end_date")]
  public DateTime? EndDate { get; init; }

  [BindFrom("customer_id")]
  public int? CustomerId { get; init; }

  [BindFrom("assigned_assistant_id")]
  public int? AssignedAssistantId { get; init; }

  [BindFrom("status")]
  public int? Status { get; init; }

  [BindFrom("source")]
  public int? Source { get; init; }
}