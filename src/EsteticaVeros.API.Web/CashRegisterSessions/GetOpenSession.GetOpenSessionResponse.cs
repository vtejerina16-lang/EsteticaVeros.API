using EsteticaVeros.API.UseCases.CashRegisterSessions;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public record GetOpenSessionResponse(CashRegisterSessionDTO? Session);