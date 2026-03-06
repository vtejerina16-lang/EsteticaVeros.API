namespace EsteticaVeros.API.Web.Appointments;

public record AppointmentDetailResponse(
  int Id,
  string AppointmentNumber,
  int CustomerId,
  string CustomerName,
  int? AssignedAssistantId,
  string? AssignedAssistantName,
  DateTime AppointmentDate,
  int EstimatedDuration,
  DateTime? ActualStartTime,
  DateTime? ActualEndTime,
  int? ActualDuration,
  int Status,
  string StatusName,
  int Source,
  string SourceName,
  decimal TotalAmount,
  decimal? DepositAmount,
  bool DepositPaid,
  int PaymentStatus,
  string PaymentStatusName,
  string? Notes,
  string? CustomerNotes,
  string? InternalNotes,
  string? CancellationReason,
  DateTime? CancelledAt,
  bool ReminderSent,
  DateTime? ReminderSentAt,
  bool ConfirmedByCustomer,
  DateTime? ConfirmedAt,
  bool IsRecurring,
  string? RecurrencePattern,
  DateTime? RecurrenceEndDate,
  DateTime CreatedAt,
  DateTime? UpdatedAt,
  List<AppointmentServiceRecord>? Services
);

public record AppointmentServiceRecord(
  int Id,
  int ServiceId,
  string ServiceName,
  int Duration,
  decimal Price,
  decimal FinalPrice,
  string Status
);