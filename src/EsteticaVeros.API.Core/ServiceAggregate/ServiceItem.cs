namespace EsteticaVeros.API.Core.ServiceAggregate;

public class ServiceItem : EntityBase
{
  public new int Id { get; private set; }
  public int ParentServiceId { get; private set; }
  public int ChildServiceId { get; private set; }
  public int Quantity { get; private set; } = 1;
  public int DisplayOrder { get; private set; }
  public string? Notes { get; private set; }
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

  // Navigation properties
  public Service ParentService { get; private set; } = null!;
  public Service ChildService { get; private set; } = null!;

  // Constructor for EF Core
  private ServiceItem() { }

  // Factory method
  public ServiceItem(
    int parentServiceId,
    int childServiceId,
    int quantity = 1,
    int displayOrder = 0,
    string? notes = null)
  {
    ParentServiceId = Guard.Against.NegativeOrZero(parentServiceId);
    ChildServiceId = Guard.Against.NegativeOrZero(childServiceId);
    Quantity = Guard.Against.NegativeOrZero(quantity);
    DisplayOrder = displayOrder;
    Notes = notes;
    CreatedAt = DateTime.UtcNow;

    // Validación: no puede referenciar a sí mismo
    if (parentServiceId == childServiceId)
    {
      throw new InvalidOperationException("A service cannot contain itself.");
    }
  }

  public ServiceItem UpdateQuantity(int quantity)
  {
    Quantity = Guard.Against.NegativeOrZero(quantity);
    return this;
  }

  public ServiceItem UpdateDisplayOrder(int displayOrder)
  {
    DisplayOrder = displayOrder;
    return this;
  }

  public ServiceItem UpdateNotes(string? notes)
  {
    Notes = notes;
    return this;
  }
}