using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashRegisters.Update;

public class UpdateCashRegisterHandler(IRepository<CashRegister> repository)
  : ICommandHandler<UpdateCashRegisterCommand, Result<CashRegisterDTO>>
{
  public async ValueTask<Result<CashRegisterDTO>> Handle(
    UpdateCashRegisterCommand command,
    CancellationToken cancellationToken)
  {
    var cashRegister = await repository.GetByIdAsync(command.CashRegisterId, cancellationToken);

    if (cashRegister == null)
    {
      return Result.NotFound();
    }

    cashRegister.UpdateInfo(command.Name, command.Location);

    await repository.UpdateAsync(cashRegister, cancellationToken);
    await repository.SaveChangesAsync(cancellationToken);

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