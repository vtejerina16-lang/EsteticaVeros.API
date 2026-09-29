namespace EsteticaVeros.API.UseCases.CashReconciliations.List;

public record ListCashReconciliationsQuery(
  int? CashRegisterSessionId = null,
  string? Status = null,
  DateTime? StartDate = null,
  DateTime? EndDate = null
) : IQuery<Result<IEnumerable<CashReconciliationDTO>>>;