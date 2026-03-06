namespace EsteticaVeros.API.UseCases.Appointments.Create;

public record CreateAppointmentCommand(
  int CustomerId,
  DateTime AppointmentDate,
  int Source, // AppointmentSource
  List<int> ServiceIds,
  int? AssignedAssistantId = null,
  string? Notes = null,
  string? CustomerNotes = null,
  string? InternalNotes = null,
  bool IsRecurring = false,
  string? RecurrencePattern = null,
  DateTime? RecurrenceEndDate = null,
  int? CreatedByUserId = null
) : ICommand<Result<AppointmentDTO>>;