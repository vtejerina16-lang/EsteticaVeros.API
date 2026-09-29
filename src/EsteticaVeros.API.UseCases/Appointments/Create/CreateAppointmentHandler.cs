using EsteticaVeros.API.Core.AppointmentAggregate;
using EsteticaVeros.API.Core.AppointmentAggregate.Specifications;
using EsteticaVeros.API.Core.AppointmentSettingsAggregate;
using EsteticaVeros.API.Core.BusinessHoursAggregate;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;
using EsteticaVeros.API.Core.ServiceAggregate;
using EsteticaVeros.API.Core.ServiceAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Appointments.Create;

public class CreateAppointmentHandler(
  IRepository<Appointment> appointmentRepository,
  IRepository<Customer> customerRepository,
  IRepository<Service> serviceRepository,
  IRepository<BusinessHour> businessHourRepository,
  IRepository<AppointmentSettings> settingsRepository,
  ILogger<CreateAppointmentHandler> logger) 
  : ICommandHandler<CreateAppointmentCommand, Result<AppointmentDTO>>
{
  public async ValueTask<Result<AppointmentDTO>> Handle(
    CreateAppointmentCommand command,
    CancellationToken cancellationToken)
  {
    try
    {
      var customerSpec = new CustomerByIdSpec(command.CustomerId);
      var customer = await customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

      if (customer == null)
      {
        return Result.NotFound("Customer not found");
      }

      if (!customer.IsActive)
      {
        return Result.Error("Customer is not active");
      }

      if (command.AppointmentDate <= DateTime.UtcNow)
      {
        return Result.Error("Appointment date must be in the future");
      }

      var services = new List<Service>();
      foreach (var serviceId in command.ServiceIds)
      {
        var serviceSpec = new ServiceByIdSpec(serviceId);
        var service = await serviceRepository.FirstOrDefaultAsync(serviceSpec, cancellationToken);

        if (service == null)
        {
          return Result.Error($"Service with ID {serviceId} not found");
        }

        if (!service.IsActive)
        {
          return Result.Error($"Service {service.Name} is not active");
        }

        services.Add(service);
      }

      if (services.Count == 0)
      {
        return Result.Error("At least one service is required");
      }

      var totalDuration = services.Sum(s => s.Duration);

      var businessHourValidation = await ValidateBusinessHours(
        command.AppointmentDate, 
        totalDuration, 
        cancellationToken);

      if (!businessHourValidation.IsSuccess)
      {
        return businessHourValidation;
      }

      var conflictValidation = await ValidateConflicts(
        command.AppointmentDate,
        totalDuration,
        command.AssignedAssistantId,
        cancellationToken);

      if (!conflictValidation.IsSuccess)
      {
        return conflictValidation;
      }

      var year = command.AppointmentDate.Year;
      var sequence = await GetNextSequenceNumber(year, cancellationToken);
      var appointmentNumber = AppointmentNumber.Generate(year, sequence);

      var source = AppointmentSource.FromValue(command.Source);
      
      var appointment = new Appointment(
        appointmentNumber,
        command.CustomerId,
        command.AppointmentDate,
        source,
        command.AssignedAssistantId,
        command.CreatedByUserId
      );

      var depositAmount = await CalculateDepositAmount(services, 0, cancellationToken);

      if (!string.IsNullOrWhiteSpace(command.Notes) || 
          !string.IsNullOrWhiteSpace(command.CustomerNotes) || 
          !string.IsNullOrWhiteSpace(command.InternalNotes))
      {
        appointment.UpdateNotes(command.Notes, command.CustomerNotes, command.InternalNotes);
      }

      if (command.IsRecurring && !string.IsNullOrWhiteSpace(command.RecurrencePattern))
      {
        appointment.SetRecurring(command.RecurrencePattern, command.RecurrenceEndDate);
      }

      var createdAppointment = await appointmentRepository.AddAsync(appointment, cancellationToken);

      foreach (var service in services)
      {
        createdAppointment.AddService(
          service.Id,
          service.Name,
          service.Duration,
          service.BasePrice,
          service.DiscountPercentage.HasValue 
            ? service.BasePrice * (service.DiscountPercentage.Value / 100) 
            : null
        );
      }

      if (depositAmount > 0)
      {
        createdAppointment.SetDeposit(depositAmount);
      }
      else
      {
        var newDepositAmount = await CalculateDepositAmount(
          services, 
          createdAppointment.TotalAmount, 
          cancellationToken);
        
        if (newDepositAmount > 0)
        {
          createdAppointment.SetDeposit(newDepositAmount);
        }
      }

      createdAppointment.ValidateBusinessRules();

      await appointmentRepository.UpdateAsync(createdAppointment, cancellationToken);

      logger.LogInformation(
        "Appointment created: {AppointmentNumber} for customer {CustomerId} on {Date}",
        createdAppointment.AppointmentNumber,
        createdAppointment.CustomerId,
        createdAppointment.AppointmentDate);

      return Result.Success(await MapToDTO(createdAppointment, customer.FullName, null, cancellationToken));
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating appointment");
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

    if (businessHour == null || !businessHour.IsActive)
    {
      return Result.Error($"No business hours configured for {dayOfWeek}");
    }

    if (!businessHour.IsWorkingDay)
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
    var endWithBuffer = appointmentDate.AddMinutes(durationMinutes + bufferTime);

    var conflictSpec = new ConflictingAppointmentsSpec(
      startWithBuffer,
      durationMinutes + (bufferTime * 2),
      assistantId
    );

    var conflicts = await appointmentRepository.ListAsync(conflictSpec, cancellationToken);

    if (conflicts.Any())
    {
      return Result.Error("Time slot is not available due to conflicting appointments");
    }

    // Validar capacidad máxima
    if (assistantId == null)
    {
      var allConflicts = await appointmentRepository.ListAsync(
        new ConflictingAppointmentsSpec(startWithBuffer, durationMinutes + (bufferTime * 2)),
        cancellationToken
      );

      if (allConflicts.Count >= settings.MaxConcurrentAppointments)
      {
        return Result.Error("Maximum concurrent appointments reached for this time slot");
      }
    }

    return Result.Success();
  }

  private async Task<decimal> CalculateDepositAmount(
    List<Service> services,
    decimal totalAmount,
    CancellationToken cancellationToken)
  {
    var allSettings = await settingsRepository.ListAsync(cancellationToken);
    var settings = allSettings.FirstOrDefault();
    if (settings == null || !settings.RequireDepositForCustomServices)
    {
      return 0;
    }

    var hasCustomService = services.Any(s => 
      s.ServiceType == Core.ServiceAggregate.ServiceType.Package || 
      s.RequiresDeposit);

    if (!hasCustomService)
    {
      return 0;
    }

    if (settings.DepositPercentage.HasValue)
    {
      return totalAmount * (settings.DepositPercentage.Value / 100);
    }

    return services
      .Where(s => s.RequiresDeposit && s.DepositAmount.HasValue)
      .Sum(s => s.DepositAmount!.Value);
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

  private async Task<AppointmentDTO> MapToDTO(
    Appointment appointment,
    string customerName,
    string? assistantName,
    CancellationToken cancellationToken)
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