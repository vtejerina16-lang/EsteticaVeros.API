namespace EsteticaVeros.API.Web.Payments;

public class GetPaymentByIdRequest
{
  public const string Route = "/api/payments/{PaymentId:int}";
  public static string BuildRoute(int paymentId) 
    => Route.Replace("{PaymentId:int}", paymentId.ToString());

  public int PaymentId { get; init; }
}