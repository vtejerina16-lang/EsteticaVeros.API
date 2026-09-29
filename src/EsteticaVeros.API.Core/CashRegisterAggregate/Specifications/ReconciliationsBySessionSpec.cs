namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class ReconciliationsBySessionSpec : Specification<CashReconciliation>
{
  public ReconciliationsBySessionSpec(int sessionId, bool includeSession = false)
  {
    Query.Where(r => r.CashRegisterSessionId == sessionId);

    if (includeSession)
    {
      Query.Include(r => r.Session);
    }

    Query.OrderByDescending(r => r.ReconciliationDate);
  }
}