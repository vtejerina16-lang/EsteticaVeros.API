using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.CashMovements.GetById;

public class GetCashMovementByIdHandler(
  IReadRepository<CashMovement> movementRepository,
  IReadRepository<CashRegisterSession> sessionRepository,
  IReadRepository<Payment> paymentRepository)
  : IQueryHandler<GetCashMovementByIdQuery, Result<CashMovementDTO>>
{
  public async ValueTask<Result<CashMovementDTO>> Handle(
    GetCashMovementByIdQuery query,
    CancellationToken cancellationToken)
  {
    var movement = await movementRepository.GetByIdAsync(query.MovementId, cancellationToken);

    if (movement == null)
    {
      return Result.NotFound();
    }

    // Obtener datos relacionados
    var session = await sessionRepository.GetByIdAsync(movement.CashRegisterSessionId, cancellationToken);
    
    Payment? payment = null;
    if (movement.PaymentId.HasValue)
    {
      payment = await paymentRepository.GetByIdAsync(movement.PaymentId.Value, cancellationToken);
    }

    var dto = new CashMovementDTO(
      movement.Id,
      movement.MovementNumber.Value,
      movement.CashRegisterSessionId,
      session?.SessionNumber.Value ?? "N/A",
      movement.PaymentId,
      payment?.PaymentNumber.Value,
      movement.Type.Value,
      movement.Type.DisplayName,
      movement.Amount,
      movement.GetSignedAmount(),
      movement.Description,
      movement.Reference,
      movement.MovementDate,
      movement.CreatedAt,
      movement.CreatedByUserId
    );

    return Result.Success(dto);
  }
}