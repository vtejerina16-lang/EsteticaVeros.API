namespace EsteticaVeros.API.UseCases.Payments;

public record PaymentDTO(
  int Id,
  string PaymentNumber,
  int? AppointmentId,
  int CustomerId,
  string CustomerName,
  int PaymentMethodId,
  string PaymentMethodName,
  int? CashRegisterSessionId,
  string? SessionNumber,
  decimal Amount,
  DateTime PaymentDate,
  string? Notes,
  string? Reference,
  string Status,
  string StatusName,
  DateTime CreatedAt,
  DateTime? UpdatedAt,
  int? CreatedByUserId
);