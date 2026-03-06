namespace EsteticaVeros.API.UseCases.Appointments.Confirm;

public record ConfirmAppointmentCommand(
  int AppointmentId
) : ICommand<Result<AppointmentDTO>>;