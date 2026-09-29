namespace EsteticaVeros.API.UseCases.CashMovements.List;

public record ListCashMovementsQuery(
  int? CashRegisterSessionId = null,
  DateTime? StartDate = null,
  DateTime? EndDate = null,
  int? Type = null
) : IQuery<Result<IEnumerable<CashMovementDTO>>>;