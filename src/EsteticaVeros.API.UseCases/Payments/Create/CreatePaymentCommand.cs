namespace EsteticaVeros.API.UseCases.Payments.Create;

public record CreatePaymentCommand(
  int CustomerId,
  int PaymentMethodId,
  decimal Amount,
  int? AppointmentId = null,
  int? CashRegisterSessionId = null,
  string? Reference = null,
  string? Notes = null,
  int? CreatedByUserId = null
) : ICommand<Result<int>>;