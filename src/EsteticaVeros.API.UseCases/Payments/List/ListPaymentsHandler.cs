using EsteticaVeros.API.Core.PaymentAggregate;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate;
using Ardalis.Result;
using EsteticaVeros.API.Core.Interfaces;

namespace EsteticaVeros.API.UseCases.Payments.List;

public class ListPaymentsHandler(
    IReadRepository<Payment> paymentRepository,
    IReadRepository<Customer> customerRepository,
    IReadRepository<CashRegisterSession> sessionRepository,
    IReadRepository<PaymentMethod> methodRepository) // 🌟 Inyectamos el repositorio faltante
    : IQueryHandler<ListPaymentsQuery, Result<IEnumerable<PaymentDTO>>>
{
    public async ValueTask<Result<IEnumerable<PaymentDTO>>> Handle(
        ListPaymentsQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Obtenemos la lista base de pagos
        var payments = await paymentRepository.ListAsync(cancellationToken);

        // 2. Aplicación de Filtros Dinámicos
        if (query.CustomerId.HasValue)
        {
            payments = payments.Where(p => p.CustomerId == query.CustomerId.Value).ToList();
        }

        if (query.AppointmentId.HasValue)
        {
            payments = payments.Where(p => p.AppointmentId == query.AppointmentId.Value).ToList();
        }

        if (query.CashRegisterSessionId.HasValue)
        {
            payments = payments.Where(p => p.CashRegisterSessionId == query.CashRegisterSessionId.Value).ToList();
        }

        if (query.StartDate.HasValue)
        {
            payments = payments.Where(p => p.PaymentDate >= query.StartDate.Value).ToList();
        }

        if (query.EndDate.HasValue)
        {
            payments = payments.Where(p => p.PaymentDate < query.EndDate.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            var status = PaymentStatus.FromName(query.Status, ignoreCase: true);
            payments = payments.Where(p => p.Status == status).ToList();
        }

        // 3. Carga de Datos Relacionados (Diccionarios para evitar NullReferenceException)
        
        // Clientes
        var customers = await customerRepository.ListAsync(cancellationToken);
        var customerDict = customers.ToDictionary(c => c.Id, c => c.FullName);

        // Sesiones de Caja
        var sessions = await sessionRepository.ListAsync(cancellationToken);
        var sessionDict = sessions.ToDictionary(s => s.Id, s => s.SessionNumber.Value);

        // Métodos de Pago 🌟 (Esto soluciona el Error 500)
        var methods = await methodRepository.ListAsync(cancellationToken);
        var methodDict = methods.ToDictionary(m => m.Id, m => m.Name);

        // 4. Proyección a DTO con Blindaje contra Nulos
        var dtos = payments.Select(p => new PaymentDTO(
            p.Id,
            p.PaymentNumber.Value,
            p.AppointmentId,
            p.CustomerId,
            customerDict.GetValueOrDefault(p.CustomerId, "Cliente no encontrado"),
            p.PaymentMethodId,
            // ✅ FIX: Usamos el diccionario en lugar de p.PaymentMethod.Name
            methodDict.GetValueOrDefault(p.PaymentMethodId, "Método Manual"),
            p.CashRegisterSessionId,
            p.CashRegisterSessionId.HasValue ? sessionDict.GetValueOrDefault(p.CashRegisterSessionId.Value) : null,
            p.Amount,
            p.PaymentDate,
            p.Notes,
            p.Reference,
            p.Status.Name,
            p.Status.DisplayName,
            p.CreatedAt,
            p.UpdatedAt,
            p.CreatedByUserId
        ));

        return Result.Success(dtos);
    }
}