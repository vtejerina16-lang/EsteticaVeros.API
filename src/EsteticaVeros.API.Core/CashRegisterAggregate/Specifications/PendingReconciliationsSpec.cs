namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class PendingReconciliationsSpec : Specification<CashReconciliation>
{
  public PendingReconciliationsSpec(bool includeSession = false)
  {
    Query.Where(r => r.Status == ReconciliationStatus.Pending);

    if (includeSession)
    {
      Query.Include(r => r.Session)
        .ThenInclude(s => s.CashRegister);
    }

    Query.OrderBy(r => r.ReconciliationDate);
  }
}