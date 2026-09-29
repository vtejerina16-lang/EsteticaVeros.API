using EsteticaVeros.API.UseCases.CashRegisterSessions;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public record ListCashRegisterSessionsResponse(IEnumerable<CashRegisterSessionDTO> Sessions);