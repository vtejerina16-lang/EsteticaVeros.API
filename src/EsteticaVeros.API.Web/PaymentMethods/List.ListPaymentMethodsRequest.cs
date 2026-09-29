namespace EsteticaVeros.API.Web.PaymentMethods;

public class ListPaymentMethodsRequest
{
  public const string Route = "/api/payment-methods";

  public bool? IsActive { get; init; }
  public bool? RequiresCashRegister { get; init; }
}