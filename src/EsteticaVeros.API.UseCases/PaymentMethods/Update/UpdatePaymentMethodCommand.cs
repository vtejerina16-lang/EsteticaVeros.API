namespace EsteticaVeros.API.UseCases.PaymentMethods.Update;

public record UpdatePaymentMethodCommand(
  int PaymentMethodId,
  string Name,
  int Type,
  bool RequiresCashRegister
) : ICommand<Result<PaymentMethodDTO>>;