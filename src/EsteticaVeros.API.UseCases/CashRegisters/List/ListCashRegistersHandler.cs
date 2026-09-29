using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashRegisters.List;

public class ListCashRegistersHandler(IReadRepository<CashRegister> repository)
  : IQueryHandler<ListCashRegistersQuery, Result<IEnumerable<CashRegisterDTO>>>
{
  public async ValueTask<Result<IEnumerable<CashRegisterDTO>>> Handle(
    ListCashRegistersQuery query,
    CancellationToken cancellationToken)
  {
    var cashRegisters = await repository.ListAsync(cancellationToken);

    // Filtrar por IsActive si se especifica
    if (query.IsActive.HasValue)
    {
      cashRegisters = cashRegisters.Where(c => c.IsActive == query.IsActive.Value).ToList();
    }

    var dtos = cashRegisters.Select(c => new CashRegisterDTO(
      c.Id,
      c.Name,
      c.Location,
      c.IsActive,
      c.CreatedAt,
      c.UpdatedAt
    ));

    return Result.Success(dtos);
  }
}