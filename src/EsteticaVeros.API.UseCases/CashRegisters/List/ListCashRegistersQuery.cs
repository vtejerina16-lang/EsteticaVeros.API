namespace EsteticaVeros.API.UseCases.CashRegisters.List;

public record ListCashRegistersQuery(
  bool? IsActive = null
) : IQuery<Result<IEnumerable<CashRegisterDTO>>>;