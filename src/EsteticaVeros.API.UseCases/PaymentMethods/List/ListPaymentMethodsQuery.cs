namespace EsteticaVeros.API.UseCases.PaymentMethods.List;

public record ListPaymentMethodsQuery(
  bool? IsActive = null,
  bool? RequiresCashRegister = null
) : IQuery<Result<IEnumerable<PaymentMethodDTO>>>;