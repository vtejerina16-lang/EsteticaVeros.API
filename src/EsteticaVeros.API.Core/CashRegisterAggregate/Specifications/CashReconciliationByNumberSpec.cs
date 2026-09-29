namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class CashReconciliationByNumberSpec : Specification<CashReconciliation>, ISingleResultSpecification<CashReconciliation>
{
  public CashReconciliationByNumberSpec(ReconciliationNumber reconciliationNumber, bool includeSession = false)
  {
    Query.Where(r => r.ReconciliationNumber == reconciliationNumber);

    if (includeSession)
    {
      Query.Include(r => r.Session);
    }
  }
}