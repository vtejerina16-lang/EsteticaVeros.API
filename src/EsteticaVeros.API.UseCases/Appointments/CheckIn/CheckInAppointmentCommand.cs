namespace EsteticaVeros.API.UseCases.Appointments.CheckIn;

public record CheckInAppointmentCommand(
  int AppointmentId
) : ICommand<Result<AppointmentDTO>>;