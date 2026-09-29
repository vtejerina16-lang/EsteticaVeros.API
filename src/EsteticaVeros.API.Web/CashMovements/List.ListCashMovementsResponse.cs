using EsteticaVeros.API.UseCases.CashMovements;

namespace EsteticaVeros.API.Web.CashMovements;

public record ListCashMovementsResponse(IEnumerable<CashMovementDTO> Movements);