namespace EsteticaVeros.API.UseCases.Services.Create;

public record CreateServiceCommand(
  int ServiceType, // 1=Simple, 2=Composite, 3=Package
  int? CategoryId,
  string Name,
  string? Description,
  string? ShortDescription,
  int Duration,
  decimal BasePrice,
  decimal? OriginalPrice,
  decimal? DiscountPercentage,
  int LoyaltyPointsEarned = 0,
  bool RequiresDeposit = false,
  decimal? DepositAmount = null,
  bool IsFeatured = false,
  int DisplayOrder = 0,
  string? ImageUrl = null,
  string? Notes = null,
  string? PreparationInstructions = null,
  string? Contraindications = null,
  List<int>? ChildServiceIds = null, // Para Composite/Package
  int? CreatedByUserId = null
) : ICommand<Result<ServiceDTO>>;