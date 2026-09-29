using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.CashReconciliations.GetById;

public class GetCashReconciliationByIdHandler(IReadRepository<CashReconciliation> repository)
  : IQueryHandler<GetCashReconciliationByIdQuery, Result<CashReconciliationDTO>>
{
  public async ValueTask<Result<CashReconciliationDTO>> Handle(
    GetCashReconciliationByIdQuery query,
    CancellationToken cancellationToken)
  {
    var spec = new CashReconciliationByIdSpec(query.ReconciliationId, includeSession: true);
    var reconciliation = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (reconciliation == null)
    {
      return Result.NotFound();
    }

    var dto = new CashReconciliationDTO(
      reconciliation.Id,
      reconciliation.ReconciliationNumber.Value,
      reconciliation.CashRegisterSessionId,
      reconciliation.Session.SessionNumber.Value,
      reconciliation.ReconciliationDate,
      reconciliation.ReconciledByUserId,
      reconciliation.SystemAmount,
      reconciliation.CountedAmount,
      reconciliation.Difference,
      reconciliation.HasDifference,
      reconciliation.IsShort,
      reconciliation.IsOver,
      reconciliation.DenominationDetails,
      reconciliation.Notes,
      reconciliation.Status.Name,
      reconciliation.Status.DisplayName,
      reconciliation.CreatedAt,
      reconciliation.ApprovedByUserId,
      reconciliation.ApprovedAt
    );

    return Result.Success(dto);
  }
}