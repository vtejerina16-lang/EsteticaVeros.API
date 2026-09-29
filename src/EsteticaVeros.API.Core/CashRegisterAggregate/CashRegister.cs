namespace EsteticaVeros.API.Core.CashRegisterAggregate;

public class CashRegister : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string? Location { get; private set; }
  public bool IsActive { get; private set; } = true;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }

  // Constructor for EF Core
  private CashRegister() { }

  // Factory method
  public CashRegister(string name, string? location = null)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
    Location = location;
    CreatedAt = DateTime.UtcNow;
  }

  public CashRegister UpdateInfo(string name, string? location = null)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
    Location = location;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public CashRegister Activate()
  {
    if (IsActive) return this;
    IsActive = true;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public CashRegister Deactivate()
  {
    if (!IsActive) return this;
    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }
}