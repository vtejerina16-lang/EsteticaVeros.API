using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.CashRegisters.Create;

public class CreateCashRegisterHandler(IRepository<CashRegister> repository)
  : ICommandHandler<CreateCashRegisterCommand, Result<int>>
{
  public async ValueTask<Result<int>> Handle(
    CreateCashRegisterCommand command,
    CancellationToken cancellationToken)
  {
    // Verificar si ya existe una caja con el mismo nombre
    var spec = new CashRegisterByNameSpec(command.Name);
    var existingRegister = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (existingRegister != null)
    {
      return Result.Error($"A cash register with the name '{command.Name}' already exists.");
    }

    var cashRegister = new CashRegister(command.Name, command.Location);

    var createdCashRegister = await repository.AddAsync(cashRegister, cancellationToken);
    await repository.SaveChangesAsync(cancellationToken);

    return Result.Success(createdCashRegister.Id);
  }
}