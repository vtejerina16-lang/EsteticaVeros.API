using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate;

public class CashRegisterSession : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public SessionNumber SessionNumber { get; private set; }
  public int CashRegisterId { get; private set; }
  public int OpenedByUserId { get; private set; }
  public int? ClosedByUserId { get; private set; }
  public DateTime OpenedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? ClosedAt { get; private set; }
  public decimal InitialAmount { get; private set; }
  public decimal ExpectedAmount { get; private set; }
  public decimal? ActualAmount { get; private set; }
  public decimal? Difference { get; private set; }
  public SessionStatus Status { get; private set; } = null!;
  public string? Notes { get; private set; }

  // Navigation property
  public CashRegister CashRegister { get; private set; } = null!;

  // Computed properties
  public bool IsOpen => Status == SessionStatus.Open;
  public bool IsClosed => Status == SessionStatus.Closed || Status == SessionStatus.Reconciled;

  // Constructor for EF Core
  private CashRegisterSession() { }

  // Factory method
  public CashRegisterSession(
    SessionNumber sessionNumber,
    int cashRegisterId,
    decimal initialAmount,
    int openedByUserId)
  {
    SessionNumber = Guard.Against.Null(sessionNumber);
    CashRegisterId = Guard.Against.NegativeOrZero(cashRegisterId);
    InitialAmount = Guard.Against.Negative(initialAmount);
    ExpectedAmount = initialAmount;
    OpenedByUserId = Guard.Against.NegativeOrZero(openedByUserId);
    OpenedAt = DateTime.UtcNow;
    Status = SessionStatus.Open;

    RegisterDomainEvent(new CashRegisterSessionOpenedEvent(this));
  }

  public CashRegisterSession Close(int closedByUserId, string? notes = null)
  {
    if (Status != SessionStatus.Open)
    {
      throw new InvalidOperationException($"Cannot close session in {Status.DisplayName} status.");
    }

    Status = SessionStatus.Closed;
    ClosedAt = DateTime.UtcNow;
    ClosedByUserId = closedByUserId;
    Notes = notes;

    RegisterDomainEvent(new CashRegisterSessionClosedEvent(this));
    return this;
  }

  public CashRegisterSession RecordReconciliation(decimal actualAmount, string? notes = null)
  {
    if (Status != SessionStatus.Closed)
    {
      throw new InvalidOperationException("Session must be closed before reconciliation.");
    }

    ActualAmount = actualAmount;
    Difference = actualAmount - ExpectedAmount;
    Status = SessionStatus.Reconciled;
    Notes = notes;

    RegisterDomainEvent(new CashRegisterSessionReconciledEvent(this));
    return this;
  }

  public CashRegisterSession AddIncome(decimal amount)
  {
    if (Status != SessionStatus.Open)
    {
      throw new InvalidOperationException("Cannot add income to a closed session.");
    }

    ExpectedAmount += Guard.Against.NegativeOrZero(amount);
    return this;
  }

  public CashRegisterSession AddExpense(decimal amount)
  {
    if (Status != SessionStatus.Open)
    {
      throw new InvalidOperationException("Cannot add expense to a closed session.");
    }

    ExpectedAmount -= Guard.Against.NegativeOrZero(amount);
    return this;
  }

  public CashRegisterSession UpdateNotes(string? notes)
  {
    Notes = notes;
    return this;
  }
}