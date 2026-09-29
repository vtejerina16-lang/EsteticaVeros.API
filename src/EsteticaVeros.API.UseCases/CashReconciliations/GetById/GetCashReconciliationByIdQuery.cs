namespace EsteticaVeros.API.UseCases.CashReconciliations.GetById;

public record GetCashReconciliationByIdQuery(int ReconciliationId) : IQuery<Result<CashReconciliationDTO>>;