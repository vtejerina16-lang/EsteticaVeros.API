using EsteticaVeros.API.Core.AppointmentAggregate;
using EsteticaVeros.API.Core.AppointmentAggregate.Specifications;
using EsteticaVeros.API.Core.AppointmentSettingsAggregate;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Appointments.MarkNoShow;

public class MarkNoShowHandler(
  IRepository<Appointment> appointmentRepository,
  IRepository<Customer> customerRepository,
  IRepository<AppointmentSettings> settingsRepository,
  ILogger<MarkNoShowHandler> logger)
  : ICommandHandler<MarkNoShowCommand, Result<AppointmentDTO>>
{
  public async ValueTask<Result<AppointmentDTO>> Handle(
    MarkNoShowCommand command,
    CancellationToken cancellationToken)
  {
    var spec = new AppointmentByIdSpec(command.AppointmentId, includeServices: true);
    var appointment = await appointmentRepository.FirstOrDefaultAsync(spec, cancellationToken);

    if (appointment == null)
    {
      return Result.NotFound();
    }

    try
    {
      // Marcar como NoShow
      appointment.MarkAsNoShow(command.Reason);
      await appointmentRepository.UpdateAsync(appointment, cancellationToken);

      // Aplicar penalización si hay configuración
      var allSettings = await settingsRepository.ListAsync(cancellationToken);
      var settings = allSettings.FirstOrDefault();

      if (settings != null && settings.NoShowPenaltyPercentage > 0)
      {
        var penalty = appointment.TotalAmount * (settings.NoShowPenaltyPercentage / 100);
        
        logger.LogWarning(
          "NoShow penalty applied: {AppointmentNumber} - Penalty: {Penalty:C}",
          appointment.AppointmentNumber,
          penalty);

        // TODO: Registrar penalización en sistema de pagos
        // TODO: Enviar notificación al cliente
      }

      logger.LogInformation(
        "Appointment marked as NoShow: {AppointmentNumber}",
        appointment.AppointmentNumber);

      var customerSpec = new CustomerByIdSpec(appointment.CustomerId);
      var customer = await customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

      return Result.Success(MapToDTO(appointment, customer?.FullName ?? "Unknown", null));
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error marking appointment as NoShow");
      return Result.Error(ex.Message);
    }
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