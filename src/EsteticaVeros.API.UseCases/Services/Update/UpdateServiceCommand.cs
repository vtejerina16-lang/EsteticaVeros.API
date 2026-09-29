namespace EsteticaVeros.API.UseCases.Services.Update;

public record UpdateServiceCommand(
  int ServiceId,
  string Name,
  int? CategoryId,
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
  List<int>? ChildServiceIds = null // Para actualizar items en Composite/Package
) : ICommand<Result<ServiceDTO>>;