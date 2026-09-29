namespace EsteticaVeros.API.Web.Appointments;

public record AvailableSlotsResponse(
  IReadOnlyList<TimeSlotRecord> Slots
);

public record TimeSlotRecord(
  DateTime StartTime,
  DateTime EndTime,
  bool IsAvailable,
  string? UnavailableReason
);