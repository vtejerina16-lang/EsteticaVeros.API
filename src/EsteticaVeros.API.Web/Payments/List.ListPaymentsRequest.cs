namespace EsteticaVeros.API.Web.Payments;

public class ListPaymentsRequest
{
  public const string Route = "/api/payments";

  public int? CustomerId { get; init; }
  public int? AppointmentId { get; init; }
  public int? CashRegisterSessionId { get; init; }
  public DateTime? StartDate { get; init; }
  public DateTime? EndDate { get; init; }
  public string? Status { get; init; }
}