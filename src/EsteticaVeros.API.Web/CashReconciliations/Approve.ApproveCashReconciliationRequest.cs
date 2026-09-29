namespace EsteticaVeros.API.Web.CashReconciliations;

public class ApproveCashReconciliationRequest
{
  public const string Route = "/api/cash-reconciliations/{ReconciliationId:int}/approve";
  public static string BuildRoute(int reconciliationId) 
    => Route.Replace("{ReconciliationId:int}", reconciliationId.ToString());

  public int ReconciliationId { get; set; }

  public string? Notes { get; set; }
}