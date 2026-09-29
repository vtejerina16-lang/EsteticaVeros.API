namespace EsteticaVeros.API.Web.CashRegisters;

public class GetCashRegisterByIdRequest
{
  public const string Route = "/api/cash-registers/{CashRegisterId:int}";
  public static string BuildRoute(int cashRegisterId) 
    => Route.Replace("{CashRegisterId:int}", cashRegisterId.ToString());

  public int CashRegisterId { get; init; }
}