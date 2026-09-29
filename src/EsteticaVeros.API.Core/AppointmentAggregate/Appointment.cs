using EsteticaVeros.API.Core.AppointmentAggregate.Events;

namespace EsteticaVeros.API.Core.AppointmentAggregate;

public class Appointment : EntityBase, IAggregateRoot
{
  private readonly List<AppointmentService> _appointmentServices = new();

  public new int Id { get; private set; }
  public AppointmentNumber AppointmentNumber { get; private set; }
  public int CustomerId { get; private set; }
  public int? AssignedAssistantId { get; private set; }
  public DateTime AppointmentDate { get; private set; }
  public int EstimatedDuration { get; private set; }
  public DateTime? ActualStartTime { get; private set; }
  public DateTime? ActualEndTime { get; private set; }
  public int? ActualDuration { get; private set; }
  public AppointmentStatus Status { get; private set; } = null!;
  public AppointmentSource Source { get; private set; } = null!;
  public decimal TotalAmount { get; private set; }
  public decimal? DepositAmount { get; private set; }
  public bool DepositPaid { get; private set; }
  public PaymentStatus PaymentStatus { get; private set; } = null!;
  public string? Notes { get; private set; }
  public string? CustomerNotes { get; private set; }
  public string? InternalNotes { get; private set; }
  public string? CancellationReason { get; private set; }
  public DateTime? CancelledAt { get; private set; }
  public int? CancelledBy { get; private set; }
  public string? NoShowReason { get; private set; }
  public bool ReminderSent { get; private set; }
  public DateTime? ReminderSentAt { get; private set; }
  public bool ConfirmedByCustomer { get; private set; }
  public DateTime? ConfirmedAt { get; private set; }
  public bool IsRecurring { get; private set; }
  public string? RecurrencePattern { get; private set; }
  public DateTime? RecurrenceEndDate { get; private set; }
  public int? ParentAppointmentId { get; private set; }
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }
  public int? CreatedByUserId { get; private set; }

  // Navigation properties
  public IReadOnlyCollection<AppointmentService> AppointmentServices => _appointmentServices.AsReadOnly();

  // Computed properties
  public DateTime EstimatedEndTime => AppointmentDate.AddMinutes(EstimatedDuration);
  public bool IsUpcoming => Status == AppointmentStatus.Scheduled || Status == AppointmentStatus.Confirmed;
  public bool CanBeCancelled => IsUpcoming && AppointmentDate > DateTime.UtcNow;
  public bool RequiresDeposit => DepositAmount.HasValue && DepositAmount.Value > 0;

  // Constructor for EF Core
  private Appointment() { }

  // Factory method
  public Appointment(
    AppointmentNumber appointmentNumber,
    int customerId,
    DateTime appointmentDate,
    AppointmentSource source,
    int? assignedAssistantId = null,
    int? createdByUserId = null)
  {
    AppointmentNumber = Guard.Against.Null(appointmentNumber);
    CustomerId = Guard.Against.NegativeOrZero(customerId);
    AppointmentDate = appointmentDate;
    Source = Guard.Against.Null(source);
    AssignedAssistantId = assignedAssistantId;
    Status = AppointmentStatus.Scheduled;
    PaymentStatus = PaymentStatus.Pending;
    CreatedByUserId = createdByUserId;
    CreatedAt = DateTime.UtcNow;

    // Validar que la fecha sea futura
    if (appointmentDate <= DateTime.UtcNow)
    {
      throw new InvalidOperationException("Appointment date must be in the future.");
    }

    RegisterDomainEvent(new AppointmentScheduledEvent(this));
  }

  // Reschedule date/time
  public Appointment Reschedule(DateTime newDate)
  {
    if (newDate <= DateTime.UtcNow)
    {
      throw new InvalidOperationException("Appointment date must be in the future.");
    }

    AppointmentDate = newDate;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new AppointmentRescheduledEvent(this));
    return this;
  }

  // Add service to appointment
  public Appointment AddService(
    int serviceId,
    string serviceName,
    int duration,
    decimal price,
    decimal? discount = null)
  {
    var displayOrder = _appointmentServices.Count;
    
    var appointmentService = new AppointmentService(
      Id,
      serviceId,
      serviceName,
      duration,
      price,
      discount,
      displayOrder
    );

    _appointmentServices.Add(appointmentService);

    // Recalcular duración y monto total
    RecalculateTotals();
    UpdatedAt = DateTime.UtcNow;

    return this;
  }

  // Remove service
  public Appointment RemoveService(int serviceId)
  {
    var service = _appointmentServices.FirstOrDefault(s => s.ServiceId == serviceId);
    if (service != null)
    {
      _appointmentServices.Remove(service);
      RecalculateTotals();
      UpdatedAt = DateTime.UtcNow;
    }

    return this;
  }

  // Update deposit
  public Appointment SetDeposit(decimal depositAmount, bool depositPaid = false)
  {
    DepositAmount = Guard.Against.Negative(depositAmount);
    DepositPaid = depositPaid;
    
    if (depositPaid)
    {
      PaymentStatus = PaymentStatus.DepositPaid;
    }

    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  // Confirm appointment
  public Appointment Confirm()
  {
    if (Status != AppointmentStatus.Scheduled)
    {
      throw new InvalidOperationException($"Cannot confirm appointment in {Status.DisplayName} status.");
    }

    Status = AppointmentStatus.Confirmed;
    ConfirmedByCustomer = true;
    ConfirmedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new AppointmentConfirmedEvent(this));
    return this;
  }

  // Check-in
  public Appointment CheckIn()
  {
    if (Status != AppointmentStatus.Scheduled &&
        Status != AppointmentStatus.Confirmed)
    {
        throw new InvalidOperationException(
            $"Cannot check-in appointment in {Status.DisplayName} status.");
    }

    Status = AppointmentStatus.CheckedIn;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new AppointmentCheckedInEvent(this));

    return this;
  }

  // Start appointment
  public Appointment Start()
  {
    if (Status != AppointmentStatus.CheckedIn)
    {
      throw new InvalidOperationException($"Cannot start appointment in {Status.DisplayName} status.");
    }

    Status = AppointmentStatus.InProgress;
    ActualStartTime = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;

    // Start first service
    var firstService = _appointmentServices.OrderBy(s => s.DisplayOrder).FirstOrDefault();
    firstService?.Start();

    RegisterDomainEvent(new AppointmentStartedEvent(this));
    return this;
  }

  // Complete appointment
  public Appointment Complete()
  {
    if (Status != AppointmentStatus.InProgress)
    {
      throw new InvalidOperationException($"Cannot complete appointment in {Status.DisplayName} status.");
    }

    // Validar que todos los servicios estén completados o omitidos
    if (_appointmentServices.Any(s => s.Status == ServiceStatus.Pending || s.Status == ServiceStatus.InProgress))
    {
      throw new InvalidOperationException("All services must be completed or skipped before completing appointment.");
    }

    Status = AppointmentStatus.Completed;
    ActualEndTime = DateTime.UtcNow;
    ActualDuration = (int)(ActualEndTime.Value - ActualStartTime!.Value).TotalMinutes;
    PaymentStatus = PaymentStatus.Paid;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new AppointmentCompletedEvent(this));
    return this;
  }

  // Cancel appointment
  public Appointment Cancel(string reason, int? cancelledBy = null)
  {
    if (!CanBeCancelled)
    {
      throw new InvalidOperationException("Appointment cannot be cancelled.");
    }

    Status = AppointmentStatus.Cancelled;
    CancellationReason = Guard.Against.NullOrWhiteSpace(reason);
    CancelledAt = DateTime.UtcNow;
    CancelledBy = cancelledBy;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new AppointmentCancelledEvent(this, reason));
    return this;
  }

  // Mark as NoShow
  public Appointment MarkAsNoShow(string? reason = null)
  {
    if (Status != AppointmentStatus.Scheduled && 
        Status != AppointmentStatus.Confirmed && 
        Status != AppointmentStatus.CheckedIn)
    {
      throw new InvalidOperationException($"Cannot mark as no-show in {Status.DisplayName} status.");
    }

    Status = AppointmentStatus.NoShow;
    NoShowReason = reason;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new AppointmentNoShowEvent(this));
    return this;
  }

  // Reschedule
  public Appointment MarkAsRescheduled()
  {
    Status = AppointmentStatus.Rescheduled;
    UpdatedAt = DateTime.UtcNow;

    RegisterDomainEvent(new AppointmentRescheduledEvent(this));
    return this;
  }

  // Update notes
  public Appointment UpdateNotes(string? notes, string? customerNotes = null, string? internalNotes = null)
  {
    Notes = notes;
    CustomerNotes = customerNotes;
    InternalNotes = internalNotes;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  // Update assistant
  public Appointment AssignAssistant(int assistantId)
  {
    AssignedAssistantId = assistantId;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  // Mark reminder sent
  public Appointment MarkReminderSent()
  {
    ReminderSent = true;
    ReminderSentAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  // Set recurring
  public Appointment SetRecurring(string pattern, DateTime? endDate = null, int? parentAppointmentId = null)
  {
    IsRecurring = true;
    RecurrencePattern = Guard.Against.NullOrWhiteSpace(pattern);
    RecurrenceEndDate = endDate;
    ParentAppointmentId = parentAppointmentId;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  // Update payment status
  public Appointment UpdatePaymentStatus(PaymentStatus paymentStatus)
  {
    PaymentStatus = Guard.Against.Null(paymentStatus);
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  // Validate business rules
  public void ValidateBusinessRules()
  {
    if (_appointmentServices.Count == 0)
    {
      throw new InvalidOperationException("Appointment must have at least one service.");
    }

    if (EstimatedDuration < 15)
    {
      throw new InvalidOperationException("Appointment duration must be at least 15 minutes.");
    }

    if (TotalAmount <= 0)
    {
      throw new InvalidOperationException("Appointment total amount must be greater than zero.");
    }

    if (DepositAmount.HasValue && DepositAmount.Value > TotalAmount)
    {
      throw new InvalidOperationException("Deposit amount cannot be greater than total amount.");
    }
  }

  // Private helper
  private void RecalculateTotals()
  {
    EstimatedDuration = _appointmentServices.Sum(s => s.Duration);
    TotalAmount = _appointmentServices.Sum(s => s.FinalPrice);
  }
}