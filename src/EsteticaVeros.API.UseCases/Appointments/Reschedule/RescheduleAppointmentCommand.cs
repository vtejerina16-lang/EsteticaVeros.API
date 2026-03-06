namespace EsteticaVeros.API.UseCases.Appointments.Reschedule;

public record RescheduleAppointmentCommand(
  int AppointmentId,
  DateTime NewAppointmentDate,
  string Reason
) : ICommand<Result<AppointmentDTO>>;