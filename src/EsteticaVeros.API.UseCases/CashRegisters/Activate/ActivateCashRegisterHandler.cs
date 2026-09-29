using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashRegisters.Activate;

public class ActivateCashRegisterHandler(IRepository<CashRegister> repository)
  : ICommandHandler<ActivateCashRegisterCommand, Result>
{
  public async ValueTask<Result> Handle(
    ActivateCashRegisterCommand command,
    CancellationToken cancellationToken)
  {
    var cashRegister = await repository.GetByIdAsync(command.CashRegisterId, cancellationToken);

    if (cashRegister == null)
    {
      return Result.NotFound();
    }

    cashRegister.Activate();

    await repository.UpdateAsync(cashRegister, cancellationToken);
    await repository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}