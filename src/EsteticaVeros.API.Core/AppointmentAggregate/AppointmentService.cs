namespace EsteticaVeros.API.Core.AppointmentAggregate;

public class AppointmentService : EntityBase
{
  public new int Id { get; private set; }
  public int AppointmentId { get; private set; }
  public int ServiceId { get; private set; }
  public string ServiceName { get; private set; } = string.Empty;
  public int Duration { get; private set; }
  public decimal Price { get; private set; }
  public decimal? Discount { get; private set; }
  public decimal FinalPrice { get; private set; }
  public int DisplayOrder { get; private set; }
  public ServiceStatus Status { get; private set; } = null!;
  public DateTime? StartedAt { get; private set; }
  public DateTime? CompletedAt { get; private set; }
  public string? Notes { get; private set; }
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

  // Navigation property
  public Appointment Appointment { get; private set; } = null!;

  // Constructor for EF Core
  private AppointmentService() { }

  // Factory method
  public AppointmentService(
    int appointmentId,
    int serviceId,
    string serviceName,
    int duration,
    decimal price,
    decimal? discount = null,
    int displayOrder = 0)
  {
    AppointmentId = Guard.Against.NegativeOrZero(appointmentId);
    ServiceId = Guard.Against.NegativeOrZero(serviceId);
    ServiceName = Guard.Against.NullOrWhiteSpace(serviceName);
    Duration = Guard.Against.NegativeOrZero(duration);
    Price = Guard.Against.Negative(price);
    Discount = discount;
    FinalPrice = CalculateFinalPrice(price, discount);
    DisplayOrder = displayOrder;
    Status = ServiceStatus.Pending;
    CreatedAt = DateTime.UtcNow;
  }

  public AppointmentService Start()
  {
    if (Status != ServiceStatus.Pending)
    {
      throw new InvalidOperationException($"Cannot start service in {Status.DisplayName} status.");
    }

    Status = ServiceStatus.InProgress;
    StartedAt = DateTime.UtcNow;
    return this;
  }

  public AppointmentService Complete(string? notes = null)
  {
    if (Status != ServiceStatus.InProgress)
    {
      throw new InvalidOperationException($"Cannot complete service in {Status.DisplayName} status.");
    }

    Status = ServiceStatus.Completed;
    CompletedAt = DateTime.UtcNow;
    Notes = notes;
    return this;
  }

  public AppointmentService Skip(string? reason = null)
  {
    if (Status == ServiceStatus.Completed)
    {
      throw new InvalidOperationException("Cannot skip a completed service.");
    }

    Status = ServiceStatus.Skipped;
    Notes = reason;
    return this;
  }

  public AppointmentService UpdateNotes(string? notes)
  {
    Notes = notes;
    return this;
  }

  private static decimal CalculateFinalPrice(decimal price, decimal? discount)
  {
    if (!discount.HasValue || discount.Value <= 0)
      return price;

    return price - discount.Value;
  }
}