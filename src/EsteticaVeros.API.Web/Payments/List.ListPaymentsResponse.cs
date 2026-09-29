using EsteticaVeros.API.UseCases.Payments;

namespace EsteticaVeros.API.Web.Payments;

public record ListPaymentsResponse(IEnumerable<PaymentDTO> Payments);