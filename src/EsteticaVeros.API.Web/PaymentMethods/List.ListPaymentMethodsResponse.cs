using EsteticaVeros.API.UseCases.PaymentMethods;

namespace EsteticaVeros.API.Web.PaymentMethods;

public record ListPaymentMethodsResponse(IEnumerable<PaymentMethodDTO> PaymentMethods);