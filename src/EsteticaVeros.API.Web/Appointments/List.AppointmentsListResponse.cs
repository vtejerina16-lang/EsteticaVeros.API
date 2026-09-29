namespace EsteticaVeros.API.Web.Appointments;

public record AppointmentsListResponse(
  IReadOnlyList<AppointmentRecord> Appointments
);