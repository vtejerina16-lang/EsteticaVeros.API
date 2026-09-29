using EsteticaVeros.API.Core.CashRegisterAggregate.Events;

namespace EsteticaVeros.API.Core.CashRegisterAggregate;

public class CashMovement : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public MovementNumber MovementNumber { get; private set; }
  public int CashRegisterSessionId { get; private set; }
  public int? PaymentId { get; private set; }
  public MovementType Type { get; private set; } = null!;
  public decimal Amount { get; private set; }
  public string Description { get; private set; } = string.Empty;
  public string? Reference { get; private set; }
  public DateTime MovementDate { get; private set; } = DateTime.UtcNow;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public int? CreatedByUserId { get; private set; }

  // Navigation properties
  public CashRegisterSession Session { get; private set; } = null!;

  // Constructor for EF Core
  private CashMovement() { }

  // Factory method
  public CashMovement(
    MovementNumber movementNumber,
    int cashRegisterSessionId,
    MovementType type,
    decimal amount,
    string description,
    int? paymentId = null,
    string? reference = null,
    int? createdByUserId = null)
  {
    MovementNumber = Guard.Against.Null(movementNumber);
    CashRegisterSessionId = Guard.Against.NegativeOrZero(cashRegisterSessionId);
    Type = Guard.Against.Null(type);
    Amount = Guard.Against.NegativeOrZero(amount);
    Description = Guard.Against.NullOrWhiteSpace(description);
    PaymentId = paymentId;
    Reference = reference;
    MovementDate = DateTime.UtcNow;
    CreatedByUserId = createdByUserId;
    CreatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new CashMovementCreatedEvent(this));
  }

  public decimal GetSignedAmount()
  {
    return Type.IsExpense ? -Amount : Amount;
  }
}