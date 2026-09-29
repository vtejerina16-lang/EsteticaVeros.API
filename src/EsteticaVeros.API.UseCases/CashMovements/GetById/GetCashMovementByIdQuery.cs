namespace EsteticaVeros.API.UseCases.CashMovements.GetById;

public record GetCashMovementByIdQuery(int MovementId) : IQuery<Result<CashMovementDTO>>;