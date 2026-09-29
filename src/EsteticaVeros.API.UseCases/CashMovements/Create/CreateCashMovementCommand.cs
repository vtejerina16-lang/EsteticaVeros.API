namespace EsteticaVeros.API.UseCases.CashMovements.Create;

public record CreateCashMovementCommand(
  int CashRegisterSessionId,
  int Type,
  decimal Amount,
  string Description,
  string? Reference = null,
  int? CreatedByUserId = null
) : ICommand<Result<int>>;