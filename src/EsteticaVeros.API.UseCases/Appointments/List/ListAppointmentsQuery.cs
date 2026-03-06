namespace EsteticaVeros.API.UseCases.Appointments.List;

public record ListAppointmentsQuery(
  DateTime? StartDate = null,
  DateTime? EndDate = null,
  int? CustomerId = null,
  int? AssignedAssistantId = null,
  int? Status = null,
  int? Source = null
) : IQuery<Result<List<AppointmentDTO>>>;