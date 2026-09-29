namespace EsteticaVeros.API.UseCases.CashRegisters.Activate;

public record ActivateCashRegisterCommand(int CashRegisterId) : ICommand<Result>;