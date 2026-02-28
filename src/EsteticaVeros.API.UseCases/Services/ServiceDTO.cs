namespace EsteticaVeros.API.UseCases.Services;

public record ServiceDTO(
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
  List<ServiceItemDTO>? ServiceItems = null
);

public record ServiceItemDTO(
  int Id,
  int ChildServiceId,
  string ChildServiceName,
  int Quantity,
  int DisplayOrder,
  string? Notes
);