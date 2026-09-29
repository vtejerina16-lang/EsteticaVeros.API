using EsteticaVeros.API.Core.AppointmentAggregate;
using EsteticaVeros.API.Core.AppointmentAggregate.Specifications;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Appointments.Complete;

public class CompleteAppointmentHandler(
  IRepository<Appointment> appointmentRepository,
  IRepository<Customer> customerRepository,
  ILogger<CompleteAppointmentHandler> logger)
  : ICommandHandler<CompleteAppointmentCommand, Result<AppointmentDTO>>
{
  public async ValueTask<Result<AppointmentDTO>> Handle(
    CompleteAppointmentCommand command,
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
      // Completar todos los servicios pendientes
      foreach (var service in appointment.AppointmentServices)
      {
        if (service.Status == ServiceStatus.Pending || service.Status == ServiceStatus.InProgress)
        {
          service.Complete(command.Notes);
        }
      }

      // Completar la cita
      appointment.Complete();

      // Actualizar notas si se proporcionan
      if (!string.IsNullOrWhiteSpace(command.Notes))
      {
        appointment.UpdateNotes(command.Notes, null, null);
      }

      await appointmentRepository.UpdateAsync(appointment, cancellationToken);

      // Actualizar estadísticas del cliente
      var customerSpec = new CustomerByIdSpec(appointment.CustomerId);
      var customer = await customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

      if (customer != null)
      {
        // Calcular puntos de lealtad basados en servicios completados
        var totalPoints = appointment.AppointmentServices
            .Where(s => s.Status == ServiceStatus.Completed)
            .Sum(s => (int)(s.FinalPrice / 10));

        // Registrar visita (actualiza TotalVisits, TotalSpent, LastVisitDate, LoyaltyPoints)
        customer.RecordVisit(appointment.TotalAmount, totalPoints);

        await customerRepository.UpdateAsync(customer, cancellationToken);
      }

      logger.LogInformation(
        "Appointment completed: {AppointmentNumber} - Duration: {Duration} minutes",
        appointment.AppointmentNumber,
        appointment.ActualDuration);

      return Result.Success(MapToDTO(appointment, customer?.FullName ?? "Unknown", null));
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error completing appointment");
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