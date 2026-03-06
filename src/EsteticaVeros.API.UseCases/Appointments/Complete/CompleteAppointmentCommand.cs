namespace EsteticaVeros.API.UseCases.Appointments.Complete;

public record CompleteAppointmentCommand(
  int AppointmentId,
  string? Notes = null
) : ICommand<Result<AppointmentDTO>>;