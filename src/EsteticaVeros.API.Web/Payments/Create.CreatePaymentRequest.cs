using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Payments;

public class CreatePaymentRequest
{
  public const string Route = "/api/payments";
  public static string BuildRoute(int id) => $"{Route}/{id}";

  [Required]
  public int CustomerId { get; set; }

  [Required]
  public int PaymentMethodId { get; set; }

  [Required]
  [Range(0.01, double.MaxValue)]
  public decimal Amount { get; set; }

  public int? AppointmentId { get; set; }

  public int? CashRegisterSessionId { get; set; }

  public string? Reference { get; set; }

  public string? Notes { get; set; }
}