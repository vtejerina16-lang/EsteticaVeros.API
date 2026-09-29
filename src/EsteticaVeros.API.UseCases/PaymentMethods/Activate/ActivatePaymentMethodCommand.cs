namespace EsteticaVeros.API.UseCases.PaymentMethods.Activate;

public record ActivatePaymentMethodCommand(int PaymentMethodId) : ICommand<Result>;