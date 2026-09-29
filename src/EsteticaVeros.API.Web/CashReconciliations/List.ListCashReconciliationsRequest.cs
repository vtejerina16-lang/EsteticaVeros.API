namespace EsteticaVeros.API.Web.CashReconciliations;

public class ListCashReconciliationsRequest
{
  public const string Route = "/api/cash-reconciliations";

  public int? CashRegisterSessionId { get; init; }
  public string? Status { get; init; }
  public DateTime? StartDate { get; init; }
  public DateTime? EndDate { get; init; }
}