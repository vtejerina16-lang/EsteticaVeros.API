namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class CashRegisterSessionByIdSpec : Specification<CashRegisterSession>, ISingleResultSpecification<CashRegisterSession>
{
  public CashRegisterSessionByIdSpec(int sessionId, bool includeCashRegister = false)
  {
    Query.Where(s => s.Id == sessionId);

    if (includeCashRegister)
    {
      Query.Include(s => s.CashRegister);
    }
  }
}