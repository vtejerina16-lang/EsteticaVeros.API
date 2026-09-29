using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.CashRegisters;

public class CreateCashRegisterRequest
{
  public const string Route = "/api/cash-registers";
  public static string BuildRoute(int id) => $"{Route}/{id}";

  [Required]
  public string Name { get; set; } = string.Empty;

  public string? Location { get; set; }
}