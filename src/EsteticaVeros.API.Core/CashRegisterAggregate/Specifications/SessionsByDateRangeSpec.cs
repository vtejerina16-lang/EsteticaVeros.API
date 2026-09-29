namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class SessionsByDateRangeSpec : Specification<CashRegisterSession>
{
  public SessionsByDateRangeSpec(
    DateTime startDate,
    DateTime endDate,
    int? cashRegisterId = null,
    bool includeCashRegister = false)
  {
    Query.Where(s => s.OpenedAt >= startDate && s.OpenedAt < endDate);

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