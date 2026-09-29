using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.PaymentMethods;

public class CreatePaymentMethodRequest
{
  public const string Route = "/api/payment-methods";
  public static string BuildRoute(int id) => $"{Route}/{id}";

  [Required]
  public string Name { get; set; } = string.Empty;

  [Required]
  [Range(1, 6, ErrorMessage = "Type must be: 1=Cash, 2=Card, 3=Transfer, 4=QR, 5=Check, 6=Other")]
  public int Type { get; set; }

  public bool RequiresCashRegister { get; set; }
}