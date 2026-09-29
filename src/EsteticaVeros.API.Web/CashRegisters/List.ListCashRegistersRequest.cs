namespace EsteticaVeros.API.Web.CashRegisters;

public class ListCashRegistersRequest
{
  public const string Route = "/api/cash-registers";

  public bool? IsActive { get; init; }
}