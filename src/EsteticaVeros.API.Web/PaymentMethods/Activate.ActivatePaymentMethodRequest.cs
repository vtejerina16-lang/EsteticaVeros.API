namespace EsteticaVeros.API.Web.PaymentMethods;

public class ActivatePaymentMethodRequest
{
  public const string Route = "/api/payment-methods/{PaymentMethodId:int}/activate";
  public static string BuildRoute(int paymentMethodId) 
    => Route.Replace("{PaymentMethodId:int}", paymentMethodId.ToString());

  public int PaymentMethodId { get; init; }
}