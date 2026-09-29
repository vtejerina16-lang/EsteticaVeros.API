namespace EsteticaVeros.API.UseCases.Payments.Refund;

public record RefundPaymentCommand(
  int PaymentId,
  string? Reason = null
) : ICommand<Result>;