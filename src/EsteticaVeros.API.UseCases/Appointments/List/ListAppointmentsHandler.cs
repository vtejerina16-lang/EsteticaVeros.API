using Ardalis.Specification;
using EsteticaVeros.API.Core.AppointmentAggregate;
using EsteticaVeros.API.Core.AppointmentAggregate.Specifications;
using EsteticaVeros.API.Core.CustomerAggregate;

namespace EsteticaVeros.API.UseCases.Appointments.List;

public class ListAppointmentsHandler(
  IRepository<Appointment> appointmentRepository,
  IRepository<Customer> customerRepository,
  ILogger<ListAppointmentsHandler> logger)
  : IQueryHandler<ListAppointmentsQuery, Result<List<AppointmentDTO>>>
{
  public async ValueTask<Result<List<AppointmentDTO>>> Handle(
    ListAppointmentsQuery query,
    CancellationToken cancellationToken)
  {
    try
    {
      Specification<Appointment> spec;

      // Si hay rango de fechas, usar esa spec
      if (query.StartDate.HasValue && query.EndDate.HasValue)
      {
        spec = new AppointmentsByDateRangeSpec(
          query.StartDate.Value,
          query.EndDate.Value,
          query.AssignedAssistantId,
          includeServices: true
        );
      }
      // Si hay customer, usar esa spec
      else if (query.CustomerId.HasValue)
      {
        spec = new AppointmentsByCustomerSpec(query.CustomerId.Value, includeServices: true);
      }
      // Si hay status, usar esa spec
      else if (query.Status.HasValue)
      {
        var status = AppointmentStatus.FromValue(query.Status.Value);
        spec = new AppointmentsByStatusSpec(status, includeServices: true);
      }
      // Por defecto, traer próximas citas
      else
      {
        spec = new UpcomingAppointmentsSpec(query.CustomerId, includeServices: true);
      }

      // Aplicar filtro de source si se especifica
      if (query.Source.HasValue)
      {
        var source = AppointmentSource.FromValue(query.Source.Value);
        spec.Query.Where(a => a.Source == source);
      }

      var appointments = await appointmentRepository.ListAsync(spec, cancellationToken);

      logger.LogInformation("Retrieved {Count} appointments", appointments.Count);

      // Obtener nombres de clientes
      var customerIds = appointments.Select(a => a.CustomerId).Distinct().ToList();
      var customers = await customerRepository.ListAsync(cancellationToken);
      var customerDict = customers.ToDictionary(c => c.Id, c => c.FullName);

      var appointmentDtos = appointments.Select(a => 
        MapToDTO(a, customerDict.GetValueOrDefault(a.CustomerId, "Unknown"), null)
      ).ToList();

      return Result.Success(appointmentDtos);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error listing appointments");
      return Result.Error("Error retrieving appointments");
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