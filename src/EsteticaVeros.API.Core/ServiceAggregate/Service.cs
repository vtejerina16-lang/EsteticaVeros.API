using EsteticaVeros.API.Core.ServiceAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceAggregate;

public class Service : EntityBase, IAggregateRoot
{
  private readonly List<ServiceItem> _serviceItems = new();

  public new int Id { get; private set; }
  public ServiceNumber ServiceNumber { get; private set; }
  public ServiceType ServiceType { get; private set; } = null!;
  public int? CategoryId { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public string? ShortDescription { get; private set; }
  public int Duration { get; private set; } // en minutos
  public decimal BasePrice { get; private set; }
  public decimal? OriginalPrice { get; private set; }
  public decimal? DiscountPercentage { get; private set; }
  public int LoyaltyPointsEarned { get; private set; }
  public bool RequiresDeposit { get; private set; }
  public decimal? DepositAmount { get; private set; }
  public bool IsActive { get; private set; } = true;
  public bool IsFeatured { get; private set; }
  public int DisplayOrder { get; private set; }
  public string? ImageUrl { get; private set; }
  public string? Notes { get; private set; }
  public string? PreparationInstructions { get; private set; }
  public string? Contraindications { get; private set; }
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }
  public int? CreatedByUserId { get; private set; }

  // Navigation properties
  public IReadOnlyCollection<ServiceItem> ServiceItems => _serviceItems.AsReadOnly();

  // Constructor for EF Core
  private Service() { }

  // Factory method for Simple Service
  public Service(
    ServiceNumber serviceNumber,
    ServiceType serviceType,
    int categoryId,
    string name,
    int duration,
    decimal basePrice,
    int? createdByUserId = null)
  {
    // Validaciones para servicio simple
    if (serviceType != ServiceType.Simple)
    {
      throw new InvalidOperationException("Use this constructor only for Simple services. Use AddCompositeService or AddPackage for other types.");
    }

    ServiceNumber = Guard.Against.Null(serviceNumber);
    ServiceType = Guard.Against.Null(serviceType);
    CategoryId = Guard.Against.NegativeOrZero(categoryId);
    Name = Guard.Against.NullOrWhiteSpace(name);
    Duration = Guard.Against.NegativeOrZero(duration);
    BasePrice = Guard.Against.Negative(basePrice);
    CreatedByUserId = createdByUserId;
    CreatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new ServiceCreatedEvent(this));
  }

  // Factory method for Composite/Package
  public static Service CreateCompositeOrPackage(
    ServiceNumber serviceNumber,
    ServiceType serviceType,
    int? categoryId,
    string name,
    int duration,
    decimal basePrice,
    int? createdByUserId = null)
  {
    if (serviceType == ServiceType.Simple)
    {
      throw new InvalidOperationException("Use the regular constructor for Simple services.");
    }

    // ✅ Permite CategoryId en Composite y Package
    if (!categoryId.HasValue || categoryId <= 0)
    {
      throw new InvalidOperationException("Services must have a valid CategoryId.");
    }

    var service = new Service
    {
      ServiceNumber = Guard.Against.Null(serviceNumber),
      ServiceType = Guard.Against.Null(serviceType),
      CategoryId = categoryId,
      Name = Guard.Against.NullOrWhiteSpace(name),
      Duration = Guard.Against.NegativeOrZero(duration),
      BasePrice = Guard.Against.Negative(basePrice),
      CreatedByUserId = createdByUserId,
      CreatedAt = DateTime.UtcNow
    };

    service.RegisterDomainEvent(new ServiceCreatedEvent(service));
    return service;
  }

  // Agregar items (solo para Composite/Package)
  public Service AddServiceItem(int childServiceId, int quantity = 1, int displayOrder = 0, string? notes = null)
  {
    if (ServiceType == ServiceType.Simple)
    {
      throw new InvalidOperationException("Cannot add service items to a Simple service.");
    }

    // Validar que no exista ya
    if (_serviceItems.Any(si => si.ChildServiceId == childServiceId))
    {
      throw new InvalidOperationException($"Service with ID {childServiceId} is already part of this composite/package.");
    }

    var item = new ServiceItem(Id, childServiceId, quantity, displayOrder, notes);
    _serviceItems.Add(item);
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new ServiceItemsChangedEvent(this));
    return this;
  }

  // Remover item
  public Service RemoveServiceItem(int childServiceId)
  {
    var item = _serviceItems.FirstOrDefault(si => si.ChildServiceId == childServiceId);
    if (item != null)
    {
      _serviceItems.Remove(item);
      UpdatedAt = DateTime.UtcNow;
      RegisterDomainEvent(new ServiceItemsChangedEvent(this));
    }

    return this;
  }

  // Validar reglas de negocio
  public void ValidateBusinessRules()
  {
    // Composite/Package deben tener al menos 2 items
    if ((ServiceType == ServiceType.Composite || ServiceType == ServiceType.Package) && _serviceItems.Count < 2)
    {
      throw new InvalidOperationException($"{ServiceType.DisplayName} must have at least 2 service items.");
    }

    // Composite/Package no pueden tener más de 10 items
    if ((ServiceType == ServiceType.Composite || ServiceType == ServiceType.Package) && _serviceItems.Count > 10)
    {
      throw new InvalidOperationException($"{ServiceType.DisplayName} cannot have more than 10 service items.");
    }
  }

  // Update methods
  public Service UpdateCategory(int? categoryId)
  {
    // ✅ Se removió la condición que forzaba CategoryId = null cuando el tipo era Package
    if (!categoryId.HasValue || categoryId <= 0)
    {
      throw new InvalidOperationException("La categoría es obligatoria.");
    }

    CategoryId = categoryId;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Service UpdateBasicInfo(string name, string? description, string? shortDescription)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
    Description = description;
    ShortDescription = shortDescription;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new ServiceUpdatedEvent(this));
    return this;
  }

  public Service UpdatePricing(decimal basePrice, decimal? originalPrice = null, decimal? discountPercentage = null)
  {
    BasePrice = Guard.Against.Negative(basePrice);
    OriginalPrice = originalPrice;
    DiscountPercentage = discountPercentage;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new ServicePriceChangedEvent(this, basePrice));
    return this;
  }

  public Service UpdateDuration(int duration)
  {
    Duration = Guard.Against.NegativeOrZero(duration);
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Service UpdateLoyaltyPoints(int points)
  {
    LoyaltyPointsEarned = Guard.Against.Negative(points);
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Service UpdateDepositSettings(bool requiresDeposit, decimal? depositAmount = null)
  {
    RequiresDeposit = requiresDeposit;
    DepositAmount = requiresDeposit ? Guard.Against.Negative(depositAmount ?? 0) : null;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Service UpdateDisplaySettings(bool isFeatured, int displayOrder, string? imageUrl = null)
  {
    IsFeatured = isFeatured;
    DisplayOrder = displayOrder;
    ImageUrl = imageUrl;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Service UpdateInstructions(string? preparationInstructions, string? contraindications, string? notes)
  {
    PreparationInstructions = preparationInstructions;
    Contraindications = contraindications;
    Notes = notes;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Service Activate()
  {
    if (IsActive) return this;
    IsActive = true;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public Service Deactivate()
  {
    if (!IsActive) return this;
    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    RegisterDomainEvent(new ServiceDeactivatedEvent(this));
    return this;
  }
}