using EsteticaVeros.API.Core.AppointmentAggregate;
using EsteticaVeros.API.Core.AppointmentAggregate.Specifications;
using EsteticaVeros.API.Core.AppointmentSettingsAggregate;
using EsteticaVeros.API.Core.BusinessHoursAggregate;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Appointments.Reschedule;

public class RescheduleAppointmentHandler(
  IRepository<Appointment> appointmentRepository,
  IRepository<Customer> customerRepository,
  IRepository<BusinessHour> businessHourRepository,
  IRepository<AppointmentSettings> settingsRepository,
  ILogger<RescheduleAppointmentHandler> logger)
  : ICommandHandler<RescheduleAppointmentCommand, Result<AppointmentDTO>>
{
  public async ValueTask<Result<AppointmentDTO>> Handle(
    RescheduleAppointmentCommand command,
    CancellationToken cancellationToken)
  {
    var spec = new AppointmentByIdSpec(command.AppointmentId, includeServices: true);
    var oldAppointment = await appointmentRepository.FirstOrDefaultAsync(spec, cancellationToken);

    if (oldAppointment == null)
    {
      return Result.NotFound();
    }

    if (oldAppointment.Status != AppointmentStatus.Scheduled && 
        oldAppointment.Status != AppointmentStatus.Confirmed)
    {
      return Result.Error("Only scheduled or confirmed appointments can be rescheduled");
    }

    // Validar nueva fecha futura
    if (command.NewAppointmentDate <= DateTime.UtcNow)
    {
      return Result.Error("New appointment date must be in the future");
    }

    // Validar horario de trabajo
    var businessHourValidation = await ValidateBusinessHours(
      command.NewAppointmentDate,
      oldAppointment.EstimatedDuration,
      cancellationToken);

    if (!businessHourValidation.IsSuccess)
    {
      return businessHourValidation;
    }

    // Validar conflictos
    var conflictValidation = await ValidateConflicts(
      command.NewAppointmentDate,
      oldAppointment.EstimatedDuration,
      oldAppointment.AssignedAssistantId,
      command.AppointmentId,
      cancellationToken);

    if (!conflictValidation.IsSuccess)
    {
      return conflictValidation;
    }

    try
    {
      // Marcar la cita anterior como reprogramada
      oldAppointment.MarkAsRescheduled();
      await appointmentRepository.UpdateAsync(oldAppointment, cancellationToken);

      // Crear nueva cita con los mismos servicios
      var year = command.NewAppointmentDate.Year;
      var sequence = await GetNextSequenceNumber(year, cancellationToken);
      var appointmentNumber = AppointmentNumber.Generate(year, sequence);

      var newAppointment = new Appointment(
        appointmentNumber,
        oldAppointment.CustomerId,
        command.NewAppointmentDate,
        oldAppointment.Source,
        oldAppointment.AssignedAssistantId,
        null
      );

      // Guardar para obtener ID
      var createdAppointment = await appointmentRepository.AddAsync(newAppointment, cancellationToken);

      // Copiar servicios
      foreach (var service in oldAppointment.AppointmentServices.OrderBy(s => s.DisplayOrder))
      {
        createdAppointment.AddService(
          service.ServiceId,
          service.ServiceName,
          service.Duration,
          service.Price,
          service.Discount
        );
      }

      // Copiar depósito si lo había
      if (oldAppointment.DepositAmount.HasValue && oldAppointment.DepositPaid)
      {
        createdAppointment.SetDeposit(oldAppointment.DepositAmount.Value, depositPaid: true);
      }

      // Copiar notas
      createdAppointment.UpdateNotes(
        $"Reprogramada desde {oldAppointment.AppointmentNumber.Value}. Razón: {command.Reason}",
        oldAppointment.CustomerNotes,
        oldAppointment.InternalNotes
      );

      // Guardar con servicios
      await appointmentRepository.UpdateAsync(createdAppointment, cancellationToken);

      logger.LogInformation(
        "Appointment rescheduled: {OldNumber} -> {NewNumber}",
        oldAppointment.AppointmentNumber,
        createdAppointment.AppointmentNumber);

      var customerSpec = new CustomerByIdSpec(createdAppointment.CustomerId);
      var customer = await customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

      return Result.Success(MapToDTO(createdAppointment, customer?.FullName ?? "Unknown", null));
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error rescheduling appointment");
      return Result.Error(ex.Message);
    }
  }

  private async Task<Result> ValidateBusinessHours(
    DateTime appointmentDate,
    int durationMinutes,
    CancellationToken cancellationToken)
  {
    var dayOfWeek = appointmentDate.DayOfWeek;
    var businessHours = await businessHourRepository.ListAsync(cancellationToken);
    var businessHour = businessHours.FirstOrDefault(bh => bh.DayOfWeek == dayOfWeek);

    if (businessHour == null || !businessHour.IsActive || !businessHour.IsWorkingDay)
    {
      return Result.Error($"{dayOfWeek} is not a working day");
    }

    var time = appointmentDate.TimeOfDay;
    if (!businessHour.CanAccommodateAppointment(time, durationMinutes))
    {
      return Result.Error("Appointment time is outside business hours");
    }

    return Result.Success();
  }

  private async Task<Result> ValidateConflicts(
    DateTime appointmentDate,
    int durationMinutes,
    int? assistantId,
    int excludeAppointmentId,
    CancellationToken cancellationToken)
  {
    var allSettings = await settingsRepository.ListAsync(cancellationToken);
    var settings = allSettings.FirstOrDefault();

    if (settings == null)
    {
      return Result.Error("Appointment settings not configured");
    }

    var bufferTime = settings.BufferTime;
    var startWithBuffer = appointmentDate.AddMinutes(-bufferTime);

    var conflictSpec = new ConflictingAppointmentsSpec(
      startWithBuffer,
      durationMinutes + (bufferTime * 2),
      assistantId,
      excludeAppointmentId
    );

    var conflicts = await appointmentRepository.ListAsync(conflictSpec, cancellationToken);

    if (conflicts.Any())
    {
      return Result.Error("Time slot is not available");
    }

    return Result.Success();
  }

  private async Task<int> GetNextSequenceNumber(int year, CancellationToken cancellationToken)
  {
    var appointments = await appointmentRepository.ListAsync(cancellationToken);
    
    var highestSequence = appointments
      .Where(a => a.AppointmentNumber.Value.StartsWith($"APT-{year}"))
      .Select(a =>
      {
        var parts = a.AppointmentNumber.Value.Split('-');
        return parts.Length == 3 && int.TryParse(parts[2], out var seq) ? seq : 0;
      })
      .DefaultIfEmpty(0)
      .Max();

    return highestSequence + 1;
  }

  private static AppointmentDTO MapToDTO(
    Appointment appointment,
    string customerName,
    string? assistantName)
  {
    var services = appointment.AppointmentServices.Select(s => new AppointmentServiceDTO(
      s.Id,
      s.ServiceId,
      s.ServiceName,
      s.Duration,
      s.Price,
      s.Discount,
      s.FinalPrice,
      s.DisplayOrder,
      s.Status.Value,
      s.Status.DisplayName,
      s.StartedAt,
      s.CompletedAt,
      s.Notes
    )).ToList();

    return new AppointmentDTO(
      appointment.Id,
      appointment.AppointmentNumber.Value,
      appointment.CustomerId,
      customerName,
      appointment.AssignedAssistantId,
      assistantName,
      appointment.AppointmentDate,
      appointment.EstimatedDuration,
      appointment.ActualStartTime,
      appointment.ActualEndTime,
      appointment.ActualDuration,
      appointment.Status.Value,
      appointment.Status.DisplayName,
      appointment.Source.Value,
      appointment.Source.DisplayName,
      appointment.TotalAmount,
      appointment.DepositAmount,
      appointment.DepositPaid,
      appointment.PaymentStatus.Value,
      appointment.PaymentStatus.DisplayName,
      appointment.Notes,
      appointment.CustomerNotes,
      appointment.InternalNotes,
      appointment.CancellationReason,
      appointment.CancelledAt,
      appointment.ReminderSent,
      appointment.ReminderSentAt,
      appointment.ConfirmedByCustomer,
      appointment.ConfirmedAt,
      appointment.IsRecurring,
      appointment.RecurrencePattern,
      appointment.RecurrenceEndDate,
      appointment.CreatedAt,
      appointment.UpdatedAt,
      services
    );
  }
}