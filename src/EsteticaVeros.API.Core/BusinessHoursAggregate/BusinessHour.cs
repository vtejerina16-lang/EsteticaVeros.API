namespace EsteticaVeros.API.Core.BusinessHoursAggregate;

public class BusinessHour : EntityBase, IAggregateRoot
{
  public new int Id { get; private set; }
  public DayOfWeek DayOfWeek { get; private set; }
  public bool IsWorkingDay { get; private set; }
  public TimeSpan? MorningStartTime { get; private set; }
  public TimeSpan? MorningEndTime { get; private set; }
  public TimeSpan? AfternoonStartTime { get; private set; }
  public TimeSpan? AfternoonEndTime { get; private set; }
  public bool IsActive { get; private set; } = true;
  public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; private set; }

  // Constructor for EF Core
  private BusinessHour() { }

  // Factory method
  public BusinessHour(
    DayOfWeek dayOfWeek,
    bool isWorkingDay,
    TimeSpan? morningStart = null,
    TimeSpan? morningEnd = null,
    TimeSpan? afternoonStart = null,
    TimeSpan? afternoonEnd = null)
  {
    DayOfWeek = dayOfWeek;
    IsWorkingDay = isWorkingDay;
    MorningStartTime = morningStart;
    MorningEndTime = morningEnd;
    AfternoonStartTime = afternoonStart;
    AfternoonEndTime = afternoonEnd;
    CreatedAt = DateTime.UtcNow;

    ValidateBusinessHours();
  }

  public BusinessHour UpdateWorkingHours(
    bool isWorkingDay,
    TimeSpan? morningStart = null,
    TimeSpan? morningEnd = null,
    TimeSpan? afternoonStart = null,
    TimeSpan? afternoonEnd = null)
  {
    IsWorkingDay = isWorkingDay;
    MorningStartTime = morningStart;
    MorningEndTime = morningEnd;
    AfternoonStartTime = afternoonStart;
    AfternoonEndTime = afternoonEnd;
    UpdatedAt = DateTime.UtcNow;

    ValidateBusinessHours();
    return this;
  }

  public BusinessHour Activate()
  {
    if (IsActive) return this;
    IsActive = true;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public BusinessHour Deactivate()
  {
    if (!IsActive) return this;
    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    return this;
  }

  public bool IsTimeWithinWorkingHours(TimeSpan time)
  {
    if (!IsWorkingDay || !IsActive) return false;

    // Check morning shift
    if (MorningStartTime.HasValue && MorningEndTime.HasValue)
    {
      if (time >= MorningStartTime.Value && time < MorningEndTime.Value)
        return true;
    }

    // Check afternoon shift
    if (AfternoonStartTime.HasValue && AfternoonEndTime.HasValue)
    {
      if (time >= AfternoonStartTime.Value && time < AfternoonEndTime.Value)
        return true;
    }

    return false;
  }

  public bool CanAccommodateAppointment(TimeSpan startTime, int durationMinutes)
  {
    if (!IsWorkingDay || !IsActive) return false;

    var endTime = startTime.Add(TimeSpan.FromMinutes(durationMinutes));

    // Check morning shift
    if (MorningStartTime.HasValue && MorningEndTime.HasValue)
    {
      if (startTime >= MorningStartTime.Value && endTime <= MorningEndTime.Value)
        return true;
    }

    // Check afternoon shift
    if (AfternoonStartTime.HasValue && AfternoonEndTime.HasValue)
    {
      if (startTime >= AfternoonStartTime.Value && endTime <= AfternoonEndTime.Value)
        return true;
    }

    return false;
  }

  private void ValidateBusinessHours()
  {
    if (!IsWorkingDay) return;

    // If working day, must have at least one shift
    if (!MorningStartTime.HasValue && !AfternoonStartTime.HasValue)
    {
      throw new InvalidOperationException("Working day must have at least one shift defined.");
    }

    // Validate morning shift
    if (MorningStartTime.HasValue && MorningEndTime.HasValue)
    {
      if (MorningStartTime.Value >= MorningEndTime.Value)
      {
        throw new InvalidOperationException("Morning start time must be before morning end time.");
      }
    }

    // Validate afternoon shift
    if (AfternoonStartTime.HasValue && AfternoonEndTime.HasValue)
    {
      if (AfternoonStartTime.Value >= AfternoonEndTime.Value)
      {
        throw new InvalidOperationException("Afternoon start time must be before afternoon end time.");
      }
    }

    // Validate that afternoon starts after morning ends
    if (MorningEndTime.HasValue && AfternoonStartTime.HasValue)
    {
      if (MorningEndTime.Value > AfternoonStartTime.Value)
      {
        throw new InvalidOperationException("Morning shift must end before afternoon shift starts.");
      }
    }
  }
}