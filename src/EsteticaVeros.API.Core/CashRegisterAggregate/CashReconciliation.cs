using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate;

public class CashReconciliation : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public ReconciliationNumber ReconciliationNumber { get; private set; }
  public int CashRegisterSessionId { get; private set; }
  public DateTime ReconciliationDate { get; private set; } = DateTime.UtcNow;
  public int ReconciledByUserId { get; private set; }
  public decimal SystemAmount { get; private set; }
  public decimal CountedAmount { get; private set; }
  public decimal Difference { get; private set; }
  public string? DenominationDetails { get; private set; }
  public string? Notes { get; private set; }
  public ReconciliationStatus Status { get; private set; } = null!;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public int? ApprovedByUserId { get; private set; }
  public DateTime? ApprovedAt { get; private set; }

  // Navigation properties
  public CashRegisterSession Session { get; private set; } = null!;

  // Computed properties
  public bool HasDifference => Math.Abs(Difference) > 0.01m;
  public bool IsShort => Difference < 0;
  public bool IsOver => Difference > 0;

  // Constructor for EF Core
  private CashReconciliation() { }

  // Factory method
  public CashReconciliation(
    ReconciliationNumber reconciliationNumber,
    int cashRegisterSessionId,
    decimal systemAmount,
    decimal countedAmount,
    int reconciledByUserId,
    string? denominationDetails = null,
    string? notes = null)
  {
    ReconciliationNumber = Guard.Against.Null(reconciliationNumber);
    CashRegisterSessionId = Guard.Against.NegativeOrZero(cashRegisterSessionId);
    SystemAmount = systemAmount;
    CountedAmount = Guard.Against.Negative(countedAmount);
    Difference = countedAmount - systemAmount;
    ReconciledByUserId = Guard.Against.NegativeOrZero(reconciledByUserId);
    DenominationDetails = denominationDetails;
    Notes = notes;
    ReconciliationDate = DateTime.UtcNow;
    Status = ReconciliationStatus.Pending;
    CreatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new CashReconciliationCreatedEvent(this));
  }

  public CashReconciliation Approve(int approvedByUserId, string? notes = null)
  {
    if (Status != ReconciliationStatus.Pending)
    {
      throw new InvalidOperationException($"Cannot approve reconciliation in {Status.DisplayName} status.");
    }

    Status = ReconciliationStatus.Approved;
    ApprovedByUserId = approvedByUserId;
    ApprovedAt = DateTime.UtcNow;
    Notes = notes ?? Notes;

    RegisterDomainEvent(new CashReconciliationApprovedEvent(this));
    return this;
  }

  public CashReconciliation Reject(int rejectedByUserId, string reason)
  {
    if (Status != ReconciliationStatus.Pending)
    {
      throw new InvalidOperationException($"Cannot reject reconciliation in {Status.DisplayName} status.");
    }

    Status = ReconciliationStatus.Rejected;
    ApprovedByUserId = rejectedByUserId;
    ApprovedAt = DateTime.UtcNow;
    Notes = Guard.Against.NullOrWhiteSpace(reason);

    RegisterDomainEvent(new CashReconciliationRejectedEvent(this));
    return this;
  }

  public CashReconciliation UpdateNotes(string? notes)
  {
    Notes = notes;
    return this;
  }
}