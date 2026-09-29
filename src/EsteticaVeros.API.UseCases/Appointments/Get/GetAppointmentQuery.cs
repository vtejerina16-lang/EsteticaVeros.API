namespace EsteticaVeros.API.UseCases.Appointments.Get;

public record GetAppointmentQuery(int AppointmentId) : IQuery<Result<AppointmentDTO>>;