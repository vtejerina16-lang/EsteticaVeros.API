using EsteticaVeros.API.Core.AppointmentAggregate;
using EsteticaVeros.API.Core.AppointmentAggregate.Specifications;
using EsteticaVeros.API.Core.AppointmentSettingsAggregate;
using EsteticaVeros.API.Core.BusinessHoursAggregate;

namespace EsteticaVeros.API.UseCases.Appointments.GetAvailableSlots;

public class GetAvailableSlotsHandler(
  IRepository<Appointment> appointmentRepository,
  IRepository<BusinessHour> businessHourRepository,
  IRepository<AppointmentSettings> settingsRepository,
  ILogger<GetAvailableSlotsHandler> logger)
  : IQueryHandler<GetAvailableSlotsQuery, Result<List<TimeSlotDTO>>>
{
  public async ValueTask<Result<List<TimeSlotDTO>>> Handle(
    GetAvailableSlotsQuery query,
    CancellationToken cancellationToken)
  {
    try
    {
      // Obtener configuración
      var allSettings = await settingsRepository.ListAsync(cancellationToken);
      var settings = allSettings.FirstOrDefault();

      if (settings == null)
      {
        return Result.Error("Appointment settings not configured");
      }

      // Obtener horario de trabajo para el día solicitado
      var dayOfWeek = query.Date.DayOfWeek;
      var businessHours = await businessHourRepository.ListAsync(cancellationToken);
      var businessHour = businessHours.FirstOrDefault(bh => bh.DayOfWeek == dayOfWeek);

      if (businessHour == null || !businessHour.IsActive || !businessHour.IsWorkingDay)
      {
        logger.LogInformation("{DayOfWeek} is not a working day", dayOfWeek);
        return Result.Success(new List<TimeSlotDTO>());
      }

      // Obtener citas existentes para el día
      var startOfDay = query.Date.Date;
      var endOfDay = startOfDay.AddDays(1);

      var appointmentsSpec = new AppointmentsByDateRangeSpec(
        startOfDay,
        endOfDay,
        query.AssignedAssistantId
      );

      var existingAppointments = await appointmentRepository.ListAsync(appointmentsSpec, cancellationToken);

      // Filtrar solo citas activas (no canceladas ni no-show)
      var activeAppointments = existingAppointments
        .Where(a => a.Status != AppointmentStatus.Cancelled && 
                    a.Status != AppointmentStatus.NoShow &&
                    a.Status != AppointmentStatus.Rescheduled)
        .ToList();

      // Generar slots disponibles
      var slots = GenerateTimeSlots(
        query.Date,
        businessHour,
        settings,
        query.EstimatedDuration,
        activeAppointments,
        query.AssignedAssistantId);

      logger.LogInformation(
        "Generated {Count} time slots for {Date}",
        slots.Count,
        query.Date.Date);

      return Result.Success(slots);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting available slots");
      return Result.Error("Error retrieving available slots");
    }
  }

  private List<TimeSlotDTO> GenerateTimeSlots(
    DateTime date,
    BusinessHour businessHour,
    AppointmentSettings settings,
    int estimatedDuration,
    List<Appointment> existingAppointments,
    int? assistantId)
  {
    var slots = new List<TimeSlotDTO>();
    var slotDuration = settings.SlotDuration;
    var bufferTime = settings.BufferTime;

    // Generar slots para turno mañana
    if (businessHour.MorningStartTime.HasValue && businessHour.MorningEndTime.HasValue)
    {
      GenerateSlotsForShift(
        date,
        businessHour.MorningStartTime.Value,
        businessHour.MorningEndTime.Value,
        slotDuration,
        bufferTime,
        estimatedDuration,
        existingAppointments,
        settings.MaxConcurrentAppointments,
        assistantId,
        slots);
    }

    // Generar slots para turno tarde
    if (businessHour.AfternoonStartTime.HasValue && businessHour.AfternoonEndTime.HasValue)
    {
      GenerateSlotsForShift(
        date,
        businessHour.AfternoonStartTime.Value,
        businessHour.AfternoonEndTime.Value,
        slotDuration,
        bufferTime,
        estimatedDuration,
        existingAppointments,
        settings.MaxConcurrentAppointments,
        assistantId,
        slots);
    }

    return slots;
  }

  private void GenerateSlotsForShift(
    DateTime date,
    TimeSpan shiftStart,
    TimeSpan shiftEnd,
    int slotDuration,
    int bufferTime,
    int estimatedDuration,
    List<Appointment> existingAppointments,
    int maxConcurrentAppointments,
    int? assistantId,
    List<TimeSlotDTO> slots)
  {
    var currentTime = shiftStart;

    while (currentTime < shiftEnd)
    {
      var slotStart = date.Date.Add(currentTime);
      var slotEnd = slotStart.AddMinutes(estimatedDuration);

      // Verificar que el slot completo quepa en el turno
      if (slotEnd.TimeOfDay > shiftEnd)
      {
        break;
      }

      // Verificar disponibilidad
      var (isAvailable, reason) = CheckSlotAvailability(
        slotStart,
        estimatedDuration,
        bufferTime,
        existingAppointments,
        maxConcurrentAppointments,
        assistantId);

      slots.Add(new TimeSlotDTO(
        slotStart,
        slotEnd,
        isAvailable,
        reason
      ));

      // Avanzar al siguiente slot
      currentTime = currentTime.Add(TimeSpan.FromMinutes(slotDuration));
    }
  }

  private (bool IsAvailable, string? Reason) CheckSlotAvailability(
    DateTime slotStart,
    int estimatedDuration,
    int bufferTime,
    List<Appointment> existingAppointments,
    int maxConcurrentAppointments,
    int? assistantId)
  {
    // No permitir agendar en el pasado
    if (slotStart <= DateTime.UtcNow)
    {
      return (false, "Slot is in the past");
    }

    var slotEnd = slotStart.AddMinutes(estimatedDuration);
    var startWithBuffer = slotStart.AddMinutes(-bufferTime);
    var endWithBuffer = slotEnd.AddMinutes(bufferTime);

    // Verificar conflictos
    var conflicts = existingAppointments.Where(a =>
    {
      var appointmentStart = a.AppointmentDate;
      var appointmentEnd = appointmentStart.AddMinutes(a.EstimatedDuration);

      // Verificar si hay solapamiento
      var hasOverlap = appointmentStart < endWithBuffer && appointmentEnd > startWithBuffer;

      // Si hay asistente específico, verificar solo ese asistente
      if (assistantId.HasValue && a.AssignedAssistantId.HasValue)
      {
        return hasOverlap && a.AssignedAssistantId == assistantId.Value;
      }

      return hasOverlap;
    }).ToList();

    if (conflicts.Any())
    {
      if (assistantId.HasValue)
      {
        return (false, "Assistant not available");
      }

      // Verificar capacidad máxima
      if (conflicts.Count >= maxConcurrentAppointments)
      {
        return (false, "Maximum capacity reached");
      }
    }

    return (true, null);
  }
}