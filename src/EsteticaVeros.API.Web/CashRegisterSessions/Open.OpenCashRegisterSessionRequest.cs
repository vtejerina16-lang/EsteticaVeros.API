using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class OpenCashRegisterSessionRequest
{
  public const string Route = "/api/cash-register-sessions/open";
  public static string BuildRoute(int id) => $"/api/cash-register-sessions/{id}";

  [Required]
  public int CashRegisterId { get; set; }

  [Required]
  [Range(0, double.MaxValue)]
  public decimal InitialAmount { get; set; }
}