namespace EsteticaVeros.API.UseCases.PaymentMethods.Deactivate;

public record DeactivatePaymentMethodCommand(int PaymentMethodId) : ICommand<Result>;