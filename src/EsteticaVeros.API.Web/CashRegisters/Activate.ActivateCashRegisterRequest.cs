namespace EsteticaVeros.API.Web.CashRegisters;

public class ActivateCashRegisterRequest
{
  public const string Route = "/api/cash-registers/{CashRegisterId:int}/activate";
  public static string BuildRoute(int cashRegisterId) 
    => Route.Replace("{CashRegisterId:int}", cashRegisterId.ToString());

  public int CashRegisterId { get; init; }
}