namespace EsteticaVeros.API.UseCases.Payments.Cancel;

public record CancelPaymentCommand(
  int PaymentId,
  string? Reason = null
) : ICommand<Result>;