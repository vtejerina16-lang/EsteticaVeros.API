using EsteticaVeros.API.Core.AppointmentAggregate;
using EsteticaVeros.API.Core.AppointmentAggregate.Specifications;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Appointments.CheckIn;

public class CheckInAppointmentHandler(
  IRepository<Appointment> appointmentRepository,
  IRepository<Customer> customerRepository,
  ILogger<CheckInAppointmentHandler> logger)
  : ICommandHandler<CheckInAppointmentCommand, Result<AppointmentDTO>>
{
  public async ValueTask<Result<AppointmentDTO>> Handle(
    CheckInAppointmentCommand command,
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
      appointment.CheckIn();
      await appointmentRepository.UpdateAsync(appointment, cancellationToken);

      logger.LogInformation(
        "Appointment checked-in: {AppointmentNumber}",
        appointment.AppointmentNumber);

      var customerSpec = new CustomerByIdSpec(appointment.CustomerId);
      var customer = await customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

      return Result.Success(MapToDTO(appointment, customer?.FullName ?? "Unknown", null));
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error checking in appointment");
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