namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class GetOpenSessionRequest
{
  public const string Route = "/api/cash-registers/{CashRegisterId:int}/open-session";
  public static string BuildRoute(int cashRegisterId) 
    => Route.Replace("{CashRegisterId:int}", cashRegisterId.ToString());

  public int CashRegisterId { get; init; }
}