using EsteticaVeros.API.UseCases.CashRegisters;

namespace EsteticaVeros.API.Web.CashRegisters;

public record ListCashRegistersResponse(IEnumerable<CashRegisterDTO> CashRegisters);