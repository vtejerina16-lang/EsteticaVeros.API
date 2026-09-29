namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class CashRegisterSessionByNumberSpec : Specification<CashRegisterSession>, ISingleResultSpecification<CashRegisterSession>
{
  public CashRegisterSessionByNumberSpec(SessionNumber sessionNumber, bool includeCashRegister = false)
  {
    Query.Where(s => s.SessionNumber == sessionNumber);

    if (includeCashRegister)
    {
      Query.Include(s => s.CashRegister);
    }
  }
}