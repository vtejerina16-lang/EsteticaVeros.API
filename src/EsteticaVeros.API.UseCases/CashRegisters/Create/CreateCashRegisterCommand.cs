namespace EsteticaVeros.API.UseCases.CashRegisters.Create;

public record CreateCashRegisterCommand(
  string Name,
  string? Location
) : ICommand<Result<int>>;