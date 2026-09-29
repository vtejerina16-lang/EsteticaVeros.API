namespace EsteticaVeros.API.UseCases.Payments.GetById;

public record GetPaymentByIdQuery(int PaymentId) : IQuery<Result<PaymentDTO>>;