using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class CloseCashRegisterSessionRequest
{
  public const string Route = "/api/cash-register-sessions/{SessionId:int}/close";
  public static string BuildRoute(int sessionId) 
    => Route.Replace("{SessionId:int}", sessionId.ToString());

  [Required]
  public int SessionId { get; set; }

  public string? Notes { get; set; }
}