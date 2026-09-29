namespace EsteticaVeros.API.UseCases.CashRegisters.Update;

public record UpdateCashRegisterCommand(
  int CashRegisterId,
  string Name,
  string? Location
) : ICommand<Result<CashRegisterDTO>>;