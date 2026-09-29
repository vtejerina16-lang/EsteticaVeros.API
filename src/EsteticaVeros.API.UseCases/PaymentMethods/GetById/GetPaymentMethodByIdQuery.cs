namespace EsteticaVeros.API.UseCases.PaymentMethods.GetById;

public record GetPaymentMethodByIdQuery(int PaymentMethodId) : IQuery<Result<PaymentMethodDTO>>;