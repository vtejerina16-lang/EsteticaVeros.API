namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class GetCashRegisterSessionByIdRequest
{
  public const string Route = "/api/cash-register-sessions/{SessionId:int}";
  public static string BuildRoute(int sessionId) 
    => Route.Replace("{SessionId:int}", sessionId.ToString());

  public int SessionId { get; init; }
}