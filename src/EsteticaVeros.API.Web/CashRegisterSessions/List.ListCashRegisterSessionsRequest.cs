namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class ListCashRegisterSessionsRequest
{
  public const string Route = "/api/cash-register-sessions";

  public int? CashRegisterId { get; init; }
  public DateTime? StartDate { get; init; }
  public DateTime? EndDate { get; init; }
  public string? Status { get; init; }
}