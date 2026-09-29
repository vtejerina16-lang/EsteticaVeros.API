using EsteticaVeros.API.UseCases.CashReconciliations;

namespace EsteticaVeros.API.Web.CashReconciliations;

public record ListCashReconciliationsResponse(IEnumerable<CashReconciliationDTO> Reconciliations);