using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashMovements.Create;

public class CreateCashMovementHandler(
  IRepository<CashMovement> movementRepository,
  IRepository<CashRegisterSession> sessionRepository)
  : ICommandHandler<CreateCashMovementCommand, Result<int>>
{
  public async ValueTask<Result<int>> Handle(
    CreateCashMovementCommand command,
    CancellationToken cancellationToken)
  {
    // Validar que la sesión existe y está abierta
    var session = await sessionRepository.GetByIdAsync(command.CashRegisterSessionId, cancellationToken);
    if (session == null)
    {
      return Result.NotFound("Cash register session not found");
    }

    if (!session.IsOpen)
    {
      return Result.Error("Cash register session is not open");
    }

    // Validar tipo de movimiento
    if (!MovementType.TryFromValue(command.Type, out var movementType))
    {
      return Result.Error("Invalid movement type");
    }

    // Generar número de movimiento
    var year = DateTime.UtcNow.Year;
    var allMovements = await movementRepository.ListAsync(cancellationToken);
    var movementsThisYear = allMovements.Count(m => m.CreatedAt.Year == year);
    var movementNumber = MovementNumber.Generate(year, movementsThisYear + 1);

    // Crear movimiento
    var movement = new CashMovement(
      movementNumber,
      command.CashRegisterSessionId,
      movementType,
      command.Amount,
      command.Description,
      null, // No asociado a pago (manual)
      command.Reference,
      command.CreatedByUserId
    );

    var createdMovement = await movementRepository.AddAsync(movement, cancellationToken);

    // Actualizar expected amount de la sesión
    if (movementType.IsIncome)
    {
      session.AddIncome(command.Amount);
    }
    else if (movementType.IsExpense)
    {
      session.AddExpense(command.Amount);
    }

    await movementRepository.SaveChangesAsync(cancellationToken);

    return Result.Success(createdMovement.Id);
  }
}