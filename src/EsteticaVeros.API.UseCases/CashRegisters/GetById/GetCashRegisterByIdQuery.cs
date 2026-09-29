namespace EsteticaVeros.API.UseCases.CashRegisters.GetById;

public record GetCashRegisterByIdQuery(int CashRegisterId) : IQuery<Result<CashRegisterDTO>>;