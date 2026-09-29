using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashRegisters.GetById;

public class GetCashRegisterByIdHandler(IReadRepository<CashRegister> repository)
  : IQueryHandler<GetCashRegisterByIdQuery, Result<CashRegisterDTO>>
{
  public async ValueTask<Result<CashRegisterDTO>> Handle(
    GetCashRegisterByIdQuery query,
    CancellationToken cancellationToken)
  {
    var cashRegister = await repository.GetByIdAsync(query.CashRegisterId, cancellationToken);

    if (cashRegister == null)
    {
      return Result.NotFound();
    }

    var dto = new CashRegisterDTO(
      cashRegister.Id,
      cashRegister.Name,
      cashRegister.Location,
      cashRegister.IsActive,
      cashRegister.CreatedAt,
      cashRegister.UpdatedAt
    );

    return Result.Success(dto);
  }
}