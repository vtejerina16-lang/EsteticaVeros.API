using EsteticaVeros.API.Core.ServiceCategoryAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceCategoryAggregate;

public class ServiceCategory : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public CategoryNumber CategoryNumber { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public int DisplayOrder { get; private set; }
  public string? IconName { get; private set; }
  public string? ColorCode { get; private set; }
  public bool IsActive { get; private set; } = true;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }
  public int? CreatedByUserId { get; private set; }

  // Constructor for EF Core
  private ServiceCategory() { }

  // Factory method
  public ServiceCategory(
    CategoryNumber categoryNumber,
    string name,
    string? description = null,
    int displayOrder = 0,
    int? createdByUserId = null)
  {
    CategoryNumber = Guard.Against.Null(categoryNumber);
    Name = Guard.Against.NullOrWhiteSpace(name);
    Description = description;
    DisplayOrder = displayOrder;
    CreatedByUserId = createdByUserId;
    CreatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new ServiceCategoryCreatedEvent(this));
  }

  public ServiceCategory UpdateBasicInfo(string name, string? description)
  {
    if (Name == name && Description == description) return this;

    Name = Guard.Against.NullOrWhiteSpace(name);
    Description = description;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new ServiceCategoryUpdatedEvent(this));
    return this;
  }

  public ServiceCategory UpdateDisplayOrder(int displayOrder)
  {
    if (DisplayOrder == displayOrder) return this;

    DisplayOrder = displayOrder;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public ServiceCategory UpdateVisualSettings(string? iconName, string? colorCode)
  {
    if (IconName == iconName && ColorCode == colorCode) return this;

    IconName = iconName;
    ColorCode = colorCode;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public ServiceCategory UpdateCreatedBy(int userId)
  {
    CreatedByUserId = userId;
    return this;
  }

  public ServiceCategory Activate()
  {
    if (IsActive) return this;

    IsActive = true;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public ServiceCategory Deactivate()
  {
    if (!IsActive) return this;

    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    RegisterDomainEvent(new ServiceCategoryDeactivatedEvent(this));
    return this;
  }
}