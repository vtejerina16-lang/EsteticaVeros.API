using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class CreateCashReconciliationRequest
{
  public const string Route = "/api/cash-reconciliations";
  public static string BuildRoute(int id) => $"{Route}/{id}";

  [Required]
  public int CashRegisterSessionId { get; set; }

  [Required]
  [Range(0, double.MaxValue)]
  public decimal CountedAmount { get; set; }

  public string? DenominationDetails { get; set; }

  public string? Notes { get; set; }
}