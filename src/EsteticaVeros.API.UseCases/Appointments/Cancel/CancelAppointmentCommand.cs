namespace EsteticaVeros.API.UseCases.Appointments.Cancel;

public record CancelAppointmentCommand(
  int AppointmentId,
  string CancellationReason,
  int? CancelledBy = null
) : ICommand<Result<AppointmentDTO>>;