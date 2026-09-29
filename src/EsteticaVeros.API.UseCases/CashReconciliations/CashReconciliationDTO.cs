namespace EsteticaVeros.API.UseCases.CashReconciliations;

public record CashReconciliationDTO(
  int Id,
  string ReconciliationNumber,
  int CashRegisterSessionId,
  string SessionNumber,
  DateTime ReconciliationDate,
  int ReconciledByUserId,
  decimal SystemAmount,
  decimal CountedAmount,
  decimal Difference,
  bool HasDifference,
  bool IsShort,
  bool IsOver,
  string? DenominationDetails,
  string? Notes,
  string Status,
  string StatusName,
  DateTime CreatedAt,
  int? ApprovedByUserId,
  DateTime? ApprovedAt
);