namespace EsteticaVeros.API.UseCases.Appointments.Start;

public record StartAppointmentCommand(
  int AppointmentId
) : ICommand<Result<AppointmentDTO>>;