using EsteticaVeros.API.Core.PaymentAggregate.Events;

namespace EsteticaVeros.API.Core.PaymentAggregate;

public class Payment : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public PaymentNumber PaymentNumber { get; private set; }
  public int? AppointmentId { get; private set; }
  public int CustomerId { get; private set; }
  public int PaymentMethodId { get; private set; }
  public int? CashRegisterSessionId { get; private set; }
  public decimal Amount { get; private set; }
  public DateTime PaymentDate { get; private set; } = DateTime.UtcNow;
  public string? Notes { get; private set; }
  public string? Reference { get; private set; }
  public PaymentStatus Status { get; private set; } = null!;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }
  public int? CreatedByUserId { get; private set; }

  // Navigation properties
  public PaymentMethod PaymentMethod { get; private set; } = null!;

  // Constructor for EF Core
  private Payment() { }

  // Factory method
  public Payment(
    PaymentNumber paymentNumber,
    int customerId,
    int paymentMethodId,
    decimal amount,
    int? appointmentId = null,
    int? cashRegisterSessionId = null,
    string? reference = null,
    string? notes = null,
    int? createdByUserId = null)
  {
    PaymentNumber = Guard.Against.Null(paymentNumber);
    CustomerId = Guard.Against.NegativeOrZero(customerId);
    PaymentMethodId = Guard.Against.NegativeOrZero(paymentMethodId);
    Amount = Guard.Against.NegativeOrZero(amount);
    AppointmentId = appointmentId;
    CashRegisterSessionId = cashRegisterSessionId;
    Reference = reference;
    Notes = notes;
    PaymentDate = DateTime.UtcNow;
    Status = PaymentStatus.Completed;
    CreatedByUserId = createdByUserId;
    CreatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new PaymentCreatedEvent(this));
  }

  public Payment UpdateStatus(PaymentStatus status)
  {
    Status = Guard.Against.Null(status);
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Payment Refund(string? reason = null)
  {
    if (Status != PaymentStatus.Completed)
    {
      throw new InvalidOperationException("Only completed payments can be refunded.");
    }

    Status = PaymentStatus.Refunded;
    Notes = reason;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new PaymentRefundedEvent(this));
    return this;
  }

  public Payment Cancel(string? reason = null)
  {
    if (Status == PaymentStatus.Completed)
    {
      throw new InvalidOperationException("Cannot cancel a completed payment. Use refund instead.");
    }

    Status = PaymentStatus.Cancelled;
    Notes = reason;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new PaymentCancelledEvent(this));
    return this;
  }

  public Payment UpdateNotes(string? notes)
  {
    Notes = notes;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Payment AssignToCashRegisterSession(int sessionId)
  {
    CashRegisterSessionId = Guard.Against.NegativeOrZero(sessionId);
    UpdatedAt = DateTime.UtcNow;
    return this;
  }
}