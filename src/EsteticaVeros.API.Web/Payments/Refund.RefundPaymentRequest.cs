namespace EsteticaVeros.API.Web.Payments;

public class RefundPaymentRequest
{
  public const string Route = "/api/payments/{PaymentId:int}/refund";
  public static string BuildRoute(int paymentId) 
    => Route.Replace("{PaymentId:int}", paymentId.ToString());

  public int PaymentId { get; set; }

  public string? Reason { get; set; }
}