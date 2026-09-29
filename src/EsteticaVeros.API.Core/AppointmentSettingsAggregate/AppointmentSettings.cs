namespace EsteticaVeros.API.Core.AppointmentSettingsAggregate;

public class AppointmentSettings : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public int SlotDuration { get; private set; }
  public int BufferTime { get; private set; }
  public int CancellationWindowHours { get; private set; }
  public decimal NoShowPenaltyPercentage { get; private set; }
  public bool RequireDepositForCustomServices { get; private set; }
  public decimal? DepositPercentage { get; private set; }
  public int MaxConcurrentAppointments { get; private set; }
  public bool EnableRecurringAppointments { get; private set; }
  public int ReminderHoursBefore { get; private set; }
  public int MaxAdvanceBookingDays { get; private set; }
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }

  // Constructor for EF Core
  private AppointmentSettings() { }

  // Factory method
  public AppointmentSettings(
    int slotDuration = 30,
    int bufferTime = 15,
    int cancellationWindowHours = 1,
    decimal noShowPenaltyPercentage = 10m,
    bool requireDepositForCustomServices = true,
    decimal? depositPercentage = 20m,
    int maxConcurrentAppointments = 2,
    bool enableRecurringAppointments = true,
    int reminderHoursBefore = 24,
    int maxAdvanceBookingDays = 60)
  {
    SlotDuration = Guard.Against.NegativeOrZero(slotDuration);
    BufferTime = Guard.Against.Negative(bufferTime);
    CancellationWindowHours = Guard.Against.Negative(cancellationWindowHours);
    NoShowPenaltyPercentage = Guard.Against.NegativeOrZero(noShowPenaltyPercentage);
    RequireDepositForCustomServices = requireDepositForCustomServices;
    DepositPercentage = depositPercentage;
    MaxConcurrentAppointments = Guard.Against.NegativeOrZero(maxConcurrentAppointments);
    EnableRecurringAppointments = enableRecurringAppointments;
    ReminderHoursBefore = Guard.Against.Negative(reminderHoursBefore);
    MaxAdvanceBookingDays = Guard.Against.NegativeOrZero(maxAdvanceBookingDays);
    CreatedAt = DateTime.UtcNow;

    ValidateSettings();
  }

  public AppointmentSettings UpdateSlotSettings(int slotDuration, int bufferTime)
  {
    SlotDuration = Guard.Against.NegativeOrZero(slotDuration);
    BufferTime = Guard.Against.Negative(bufferTime);
    UpdatedAt = DateTime.UtcNow;
    ValidateSettings();
    return this;
  }

  public AppointmentSettings UpdateCancellationPolicy(
    int cancellationWindowHours,
    decimal noShowPenaltyPercentage)
  {
    CancellationWindowHours = Guard.Against.Negative(cancellationWindowHours);
    NoShowPenaltyPercentage = Guard.Against.NegativeOrZero(noShowPenaltyPercentage);
    UpdatedAt = DateTime.UtcNow;
    ValidateSettings();
    return this;
  }

  public AppointmentSettings UpdateDepositSettings(
    bool requireDepositForCustomServices,
    decimal? depositPercentage)
  {
    RequireDepositForCustomServices = requireDepositForCustomServices;
    DepositPercentage = depositPercentage;
    UpdatedAt = DateTime.UtcNow;
    ValidateSettings();
    return this;
  }

  public AppointmentSettings UpdateCapacitySettings(int maxConcurrentAppointments)
  {
    MaxConcurrentAppointments = Guard.Against.NegativeOrZero(maxConcurrentAppointments);
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public AppointmentSettings UpdateRecurrenceSettings(bool enableRecurringAppointments)
  {
    EnableRecurringAppointments = enableRecurringAppointments;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public AppointmentSettings UpdateNotificationSettings(int reminderHoursBefore)
  {
    ReminderHoursBefore = Guard.Against.Negative(reminderHoursBefore);
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public AppointmentSettings UpdateBookingWindow(int maxAdvanceBookingDays)
  {
    MaxAdvanceBookingDays = Guard.Against.NegativeOrZero(maxAdvanceBookingDays);
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  private void ValidateSettings()
  {
    if (SlotDuration < 15)
    {
      throw new InvalidOperationException("Slot duration must be at least 15 minutes.");
    }

    if (NoShowPenaltyPercentage < 0 || NoShowPenaltyPercentage > 100)
    {
      throw new InvalidOperationException("NoShow penalty percentage must be between 0 and 100.");
    }

    if (DepositPercentage.HasValue && (DepositPercentage.Value < 0 || DepositPercentage.Value > 100))
    {
      throw new InvalidOperationException("Deposit percentage must be between 0 and 100.");
    }

    if (BufferTime >= SlotDuration)
    {
      throw new InvalidOperationException("Buffer time must be less than slot duration.");
    }
  }
}