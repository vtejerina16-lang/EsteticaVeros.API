namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class CashReconciliationByIdSpec : Specification<CashReconciliation>, ISingleResultSpecification<CashReconciliation>
{
  public CashReconciliationByIdSpec(int reconciliationId, bool includeSession = false)
  {
    Query.Where(r => r.Id == reconciliationId);

    if (includeSession)
    {
      Query.Include(r => r.Session);
    }
  }
}