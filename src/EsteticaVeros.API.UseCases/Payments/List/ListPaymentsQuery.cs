namespace EsteticaVeros.API.UseCases.Payments.List;

public record ListPaymentsQuery(
  int? CustomerId = null,
  int? AppointmentId = null,
  int? CashRegisterSessionId = null,
  DateTime? StartDate = null,
  DateTime? EndDate = null,
  string? Status = null
) : IQuery<Result<IEnumerable<PaymentDTO>>>;