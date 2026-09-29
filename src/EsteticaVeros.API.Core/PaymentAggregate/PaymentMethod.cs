namespace EsteticaVeros.API.Core.PaymentAggregate;

public class PaymentMethod : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public PaymentMethodType Type { get; private set; } = null!;
  public bool RequiresCashRegister { get; private set; }
  public bool IsActive { get; private set; } = true;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }

  // Constructor for EF Core
  private PaymentMethod() { }

  // Factory method
  public PaymentMethod(
    string name,
    PaymentMethodType type,
    bool requiresCashRegister = false)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
    Type = Guard.Against.Null(type);
    RequiresCashRegister = requiresCashRegister;
    CreatedAt = DateTime.UtcNow;
  }

  public PaymentMethod UpdateInfo(string name, PaymentMethodType type, bool requiresCashRegister)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
    Type = Guard.Against.Null(type);
    RequiresCashRegister = requiresCashRegister;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public PaymentMethod Activate()
  {
    if (IsActive) return this;
    IsActive = true;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public PaymentMethod Deactivate()
  {
    if (!IsActive) return this;
    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }
}