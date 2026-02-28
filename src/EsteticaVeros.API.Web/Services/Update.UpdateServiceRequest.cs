using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Services;

public class UpdateServiceRequest
{
  public const string Route = "/api/services/{ServiceId:int}";
  public static string BuildRoute(int serviceId) => Route.Replace("{ServiceId:int}", serviceId.ToString());

  public int ServiceId { get; set; }

  [Required]
  public string Name { get; set; } = string.Empty;

  public string? Description { get; set; }

  public string? ShortDescription { get; set; }

  [Required]
  public int Duration { get; set; }

  [Required]
  public decimal BasePrice { get; set; }

  public decimal? OriginalPrice { get; set; }

  public decimal? DiscountPercentage { get; set; }

  public int LoyaltyPointsEarned { get; set; } = 0;

  public bool RequiresDeposit { get; set; } = false;

  public decimal? DepositAmount { get; set; }

  public bool IsActive { get; set; } = true;

  public bool IsFeatured { get; set; } = false;

  public int DisplayOrder { get; set; } = 0;

  public string? ImageUrl { get; set; }

  public string? Notes { get; set; }

  public string? PreparationInstructions { get; set; }

  public string? Contraindications { get; set; }

  public List<int>? ChildServiceIds { get; set; }
}