namespace EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

public sealed class CashRegisterByNameSpec : Specification<CashRegister>, ISingleResultSpecification<CashRegister>
{
  public CashRegisterByNameSpec(string name)
  {
    Query.Where(c => c.Name.ToLower() == name.ToLower());
  }
}