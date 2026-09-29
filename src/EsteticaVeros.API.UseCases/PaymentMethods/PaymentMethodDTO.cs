namespace EsteticaVeros.API.UseCases.PaymentMethods;

public record PaymentMethodDTO(
  int Id,
  string Name,
  int Type,
  string TypeName,
  bool RequiresCashRegister,
  bool IsActive,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);