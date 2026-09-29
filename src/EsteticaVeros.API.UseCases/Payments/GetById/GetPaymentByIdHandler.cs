using EsteticaVeros.API.Core.PaymentAggregate;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace EsteticaVeros.API.UseCases.Payments.GetById;

public class GetPaymentByIdHandler(
    IReadRepository<Payment> paymentRepository,
    IReadRepository<Customer> customerRepository,
    IReadRepository<CashRegisterSession> sessionRepository,
    IReadRepository<PaymentMethod> methodRepository) // 🌟 1. Inyectamos el repositorio
    : IQueryHandler<GetPaymentByIdQuery, Result<PaymentDTO>>
{
    public async ValueTask<Result<PaymentDTO>> Handle(
        GetPaymentByIdQuery query,
        CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetByIdAsync(query.PaymentId, cancellationToken);

        if (payment == null)
        {
            return Result.NotFound();
        }

        // 2. Obtener datos relacionados de forma segura
        var customer = await customerRepository.GetByIdAsync(payment.CustomerId, cancellationToken);
        
        // 🌟 3. Cargar el Método de Pago (Esto evita el Error 500)
        var method = await methodRepository.GetByIdAsync(payment.PaymentMethodId, cancellationToken);
        
        CashRegisterSession? session = null;
        if (payment.CashRegisterSessionId.HasValue)
        {
            session = await sessionRepository.GetByIdAsync(payment.CashRegisterSessionId.Value, cancellationToken);
        }

        var dto = new PaymentDTO(
            payment.Id,
            payment.PaymentNumber.Value,
            payment.AppointmentId,
            payment.CustomerId,
            customer?.FullName ?? "N/A",
            payment.PaymentMethodId,
            method?.Name ?? "Manual / Otro", // ✅ 4. Usamos el objeto cargado, no el nulo
            payment.CashRegisterSessionId,
            session?.SessionNumber.Value,
            payment.Amount,
            payment.PaymentDate,
            payment.Notes,
            payment.Reference,
            payment.Status.Name,
            payment.Status.DisplayName,
            payment.CreatedAt,
            payment.UpdatedAt,
            payment.CreatedByUserId
        );

        return Result.Success(dto);
    }
}