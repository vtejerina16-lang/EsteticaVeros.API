namespace EsteticaVeros.API.Web.CashMovements;

public class ListCashMovementsRequest
{
  public const string Route = "/api/cash-movements";

  public int? CashRegisterSessionId { get; init; }
  public DateTime? StartDate { get; init; }
  public DateTime? EndDate { get; init; }
  public int? Type { get; init; }
}