namespace EsteticaVeros.API.Web.Appointments;

public record AppointmentRecord(
  int Id,
  string AppointmentNumber,
  string CustomerName,
  DateTime AppointmentDate,
  int EstimatedDuration,
  string Status,
  decimal TotalAmount
);