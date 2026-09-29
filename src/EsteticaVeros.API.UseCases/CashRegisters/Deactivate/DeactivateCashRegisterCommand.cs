namespace EsteticaVeros.API.UseCases.CashRegisters.Deactivate;

public record DeactivateCashRegisterCommand(int CashRegisterId) : ICommand<Result>;