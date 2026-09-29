using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.CashRegisters;

public class UpdateCashRegisterRequest
{
  public const string Route = "/api/cash-registers/{CashRegisterId:int}";
  public static string BuildRoute(int cashRegisterId) 
    => Route.Replace("{CashRegisterId:int}", cashRegisterId.ToString());

  public int CashRegisterId { get; init; }

  [Required]
  public string Name { get; init; } = string.Empty;

  public string? Location { get; init; }
}