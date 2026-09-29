using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class UpdatePaymentMethodRequest
{
  public const string Route = "/api/payment-methods/{PaymentMethodId:int}";
  public static string BuildRoute(int paymentMethodId) 
    => Route.Replace("{PaymentMethodId:int}", paymentMethodId.ToString());

  public int PaymentMethodId { get; init; }

  [Required]
  public string Name { get; init; } = string.Empty;

  [Required]
  [Range(1, 6)]
  public int Type { get; init; }

  public bool RequiresCashRegister { get; init; }
}