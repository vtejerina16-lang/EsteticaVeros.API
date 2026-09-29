namespace EsteticaVeros.API.Web.PaymentMethods;

public class GetPaymentMethodByIdRequest
{
  public const string Route = "/api/payment-methods/{PaymentMethodId:int}";
  public static string BuildRoute(int paymentMethodId) 
    => Route.Replace("{PaymentMethodId:int}", paymentMethodId.ToString());

  public int PaymentMethodId { get; init; }
}