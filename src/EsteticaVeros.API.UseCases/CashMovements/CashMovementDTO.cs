namespace EsteticaVeros.API.UseCases.CashMovements;

public record CashMovementDTO(
  int Id,
  string MovementNumber,
  int CashRegisterSessionId,
  string SessionNumber,
  int? PaymentId,
  string? PaymentNumber,
  int Type,
  string TypeName,
  decimal Amount,
  decimal SignedAmount,
  string Description,
  string? Reference,
  DateTime MovementDate,
  DateTime CreatedAt,
  int? CreatedByUserId
);