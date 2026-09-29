using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.CashReconciliations.List;

public class ListCashReconciliationsHandler(IReadRepository<CashReconciliation> repository)
  : IQueryHandler<ListCashReconciliationsQuery, Result<IEnumerable<CashReconciliationDTO>>>
{
  public async ValueTask<Result<IEnumerable<CashReconciliationDTO>>> Handle(
    ListCashReconciliationsQuery query,
    CancellationToken cancellationToken)
  {
    IEnumerable<CashReconciliation> reconciliations;

    // Si se filtra por sesión específica
    if (query.CashRegisterSessionId.HasValue)
    {
      var spec = new ReconciliationsBySessionSpec(query.CashRegisterSessionId.Value, includeSession: true);
      reconciliations = await repository.ListAsync(spec, cancellationToken);
    }
    // Si se filtra por estado pendiente
    else if (query.Status == "Pending")
    {
      var spec = new PendingReconciliationsSpec(includeSession: true);
      reconciliations = await repository.ListAsync(spec, cancellationToken);
    }
    // Sin filtros específicos
    else
    {
      reconciliations = await repository.ListAsync(cancellationToken);
    }

    // Filtrar por fecha
    if (query.StartDate.HasValue)
    {
      reconciliations = reconciliations.Where(r => r.ReconciliationDate >= query.StartDate.Value);
    }

    if (query.EndDate.HasValue)
    {
      reconciliations = reconciliations.Where(r => r.ReconciliationDate < query.EndDate.Value);
    }

    // Filtrar por estado
    if (!string.IsNullOrWhiteSpace(query.Status) && query.Status != "Pending")
    {
      var status = ReconciliationStatus.FromName(query.Status, ignoreCase: true);
      reconciliations = reconciliations.Where(r => r.Status == status);
    }

    var dtos = reconciliations.Select(r => new CashReconciliationDTO(
      r.Id,
      r.ReconciliationNumber.Value,
      r.CashRegisterSessionId,
      r.Session?.SessionNumber.Value ?? "N/A",
      r.ReconciliationDate,
      r.ReconciledByUserId,
      r.SystemAmount,
      r.CountedAmount,
      r.Difference,
      r.HasDifference,
      r.IsShort,
      r.IsOver,
      r.DenominationDetails,
      r.Notes,
      r.Status.Name,
      r.Status.DisplayName,
      r.CreatedAt,
      r.ApprovedByUserId,
      r.ApprovedAt
    ));

    return Result.Success(dtos);
  }
}