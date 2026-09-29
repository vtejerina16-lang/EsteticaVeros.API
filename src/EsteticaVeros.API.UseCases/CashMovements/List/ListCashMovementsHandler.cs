using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.CashMovements.List;

public class ListCashMovementsHandler(
  IReadRepository<CashMovement> movementRepository,
  IReadRepository<CashRegisterSession> sessionRepository,
  IReadRepository<Payment> paymentRepository)
  : IQueryHandler<ListCashMovementsQuery, Result<IEnumerable<CashMovementDTO>>>
{
  public async ValueTask<Result<IEnumerable<CashMovementDTO>>> Handle(
    ListCashMovementsQuery query,
    CancellationToken cancellationToken)
  {
    var movements = await movementRepository.ListAsync(cancellationToken);

    // Filtros
    if (query.CashRegisterSessionId.HasValue)
    {
      movements = movements.Where(m => m.CashRegisterSessionId == query.CashRegisterSessionId.Value).ToList();
    }

    if (query.StartDate.HasValue)
    {
      movements = movements.Where(m => m.MovementDate >= query.StartDate.Value).ToList();
    }

    if (query.EndDate.HasValue)
    {
      movements = movements.Where(m => m.MovementDate < query.EndDate.Value).ToList();
    }

    if (query.Type.HasValue)
    {
      var type = MovementType.FromValue(query.Type.Value);
      movements = movements.Where(m => m.Type == type).ToList();
    }

    // Cargar datos relacionados
    var sessionIds = movements.Select(m => m.CashRegisterSessionId).Distinct();
    var sessions = await sessionRepository.ListAsync(cancellationToken);
    var sessionDict = sessions.ToDictionary(s => s.Id, s => s.SessionNumber.Value);

    var paymentIds = movements.Where(m => m.PaymentId.HasValue)
      .Select(m => m.PaymentId!.Value)
      .Distinct();
    var payments = await paymentRepository.ListAsync(cancellationToken);
    var paymentDict = payments.ToDictionary(p => p.Id, p => p.PaymentNumber.Value);

    var dtos = movements.Select(m => new CashMovementDTO(
      m.Id,
      m.MovementNumber.Value,
      m.CashRegisterSessionId,
      sessionDict.GetValueOrDefault(m.CashRegisterSessionId, "N/A"),
      m.PaymentId,
      m.PaymentId.HasValue ? paymentDict.GetValueOrDefault(m.PaymentId.Value) : null,
      m.Type.Value,
      m.Type.DisplayName,
      m.Amount,
      m.GetSignedAmount(),
      m.Description,
      m.Reference,
      m.MovementDate,
      m.CreatedAt,
      m.CreatedByUserId
    ));

    return Result.Success(dtos);
  }
}