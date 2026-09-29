namespace EsteticaVeros.API.UseCases.Appointments.Update;

public record UpdateAppointmentCommand(
  int AppointmentId,
  DateTime? AppointmentDate = null,
  List<int>? ServiceIds = null,
  int? AssignedAssistantId = null,
  string? Notes = null,
  string? CustomerNotes = null,
  string? InternalNotes = null
) : ICommand<Result<AppointmentDTO>>;