using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class RejectCashReconciliationRequest
{
  public const string Route = "/api/cash-reconciliations/{ReconciliationId:int}/reject";
  public static string BuildRoute(int reconciliationId) 
    => Route.Replace("{ReconciliationId:int}", reconciliationId.ToString());

  [Required]
  public int ReconciliationId { get; set; }

  [Required]
  public string Reason { get; set; } = string.Empty;
}