using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Services;

public class CreateServiceRequest
{
  public const string Route = "/api/services";

  [Required]
  public int ServiceType { get; set; }

  public int? CategoryId { get; set; }

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

  public bool IsFeatured { get; set; } = false;

  public int DisplayOrder { get; set; } = 0;

  public string? ImageUrl { get; set; }

  public string? Notes { get; set; }

  public string? PreparationInstructions { get; set; }

  public string? Contraindications { get; set; }

  public List<int>? ChildServiceIds { get; set; }
}