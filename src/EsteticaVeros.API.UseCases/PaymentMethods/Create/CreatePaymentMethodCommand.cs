namespace EsteticaVeros.API.UseCases.PaymentMethods.Create;

public record CreatePaymentMethodCommand(
  string Name,
  int Type,
  bool RequiresCashRegister
) : ICommand<Result<int>>;