namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class OpenSessionsSpec : Specification<CashRegisterSession>
{
  public OpenSessionsSpec(int? cashRegisterId = null, bool includeCashRegister = false)
  {
    Query.Where(s => s.Status == SessionStatus.Open);

    if (cashRegisterId.HasValue)
    {
      Query.Where(s => s.CashRegisterId == cashRegisterId.Value);
    }

    if (includeCashRegister)
    {
      Query.Include(s => s.CashRegister);
    }

    Query.OrderByDescending(s => s.OpenedAt);
  }
}