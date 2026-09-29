namespace EsteticaVeros.API.UseCases.Appointments.MarkNoShow;

public record MarkNoShowCommand(
  int AppointmentId,
  string? Reason = null
) : ICommand<Result<AppointmentDTO>>;