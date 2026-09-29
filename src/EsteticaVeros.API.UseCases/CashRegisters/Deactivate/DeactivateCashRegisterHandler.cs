using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashRegisters.Deactivate;

public class DeactivateCashRegisterHandler(IRepository<CashRegister> repository)
  : ICommandHandler<DeactivateCashRegisterCommand, Result>
{
  public async ValueTask<Result> Handle(
    DeactivateCashRegisterCommand command,
    CancellationToken cancellationToken)
  {
    var cashRegister = await repository.GetByIdAsync(command.CashRegisterId, cancellationToken);

    if (cashRegister == null)
    {
      return Result.NotFound();
    }

    cashRegister.Deactivate();

    await repository.UpdateAsync(cashRegister, cancellationToken);
    await repository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}