namespace EsteticaVeros.API.Web.CashReconciliations;

public class GetCashReconciliationByIdRequest
{
  public const string Route = "/api/cash-reconciliations/{ReconciliationId:int}";
  public static string BuildRoute(int reconciliationId) 
    => Route.Replace("{ReconciliationId:int}", reconciliationId.ToString());

  public int ReconciliationId { get; init; }
}