using EsteticaVeros.API.UseCases.Services;

namespace EsteticaVeros.API.Web.Services;

public record ServiceDetailResponse(
  int Id,
  string ServiceNumber,
  int ServiceType,
  string ServiceTypeName,
  int? CategoryId,
  string Name,
  string? Description,
  string? ShortDescription,
  int Duration,
  decimal BasePrice,
  decimal? OriginalPrice,
  decimal? DiscountPercentage,
  int LoyaltyPointsEarned,
  bool RequiresDeposit,
  decimal? DepositAmount,
  bool IsActive,
  bool IsFeatured,
  int DisplayOrder,
  string? ImageUrl,
  string? Notes,
  string? PreparationInstructions,
  string? Contraindications,
  DateTime CreatedAt,
  DateTime? UpdatedAt,
  List<ServiceItemDTO>? ServiceItems
);