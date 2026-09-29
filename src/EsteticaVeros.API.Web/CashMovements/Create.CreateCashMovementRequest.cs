using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.CashMovements;

public class CreateCashMovementRequest
{
  public const string Route = "/api/cash-movements";
  public static string BuildRoute(int id) => $"{Route}/{id}";

  [Required]
  public int CashRegisterSessionId { get; set; }

  [Required]
  [Range(1, 6, ErrorMessage = "Type must be: 1=Income, 2=Expense, 3=Withdrawal, 4=Deposit, 5=Transfer, 6=Adjustment")]
  public int Type { get; set; }

  [Required]
  [Range(0.01, double.MaxValue)]
  public decimal Amount { get; set; }

  [Required]
  public string Description { get; set; } = string.Empty;

  public string? Reference { get; set; }
}