namespace EsteticaVeros.API.UseCases.Appointments.GetAvailableSlots;

public record GetAvailableSlotsQuery(
  DateTime Date,
  int EstimatedDuration,
  int? AssignedAssistantId = null
) : IQuery<Result<List<TimeSlotDTO>>>;

public record TimeSlotDTO(
  DateTime StartTime,
  DateTime EndTime,
  bool IsAvailable,
  string? UnavailableReason = null
);