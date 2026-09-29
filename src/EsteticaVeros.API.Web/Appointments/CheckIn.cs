using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.CheckIn;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class CheckIn(IMediator mediator)
  : Endpoint<CheckInAppointmentRequest,
             Results<Ok<CheckInAppointmentResponse>,
                     NotFound,
                     ProblemHttpResult>,
             CheckInAppointmentMapper>
{
  public override void Configure()
  {
    Post(CheckInAppointmentRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Check-in para cita";
      s.Description = "Marca que el cliente ha llegado para su cita.";
      s.Responses[200] = "Cliente registrado exitosamente";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[400] = "La cita no puede ser registrada";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<CheckInAppointmentResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(CheckInAppointmentRequest request, CancellationToken ct)
  {
    var command = new CheckInAppointmentCommand(request.AppointmentId);
    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class CheckInAppointmentMapper
  : Mapper<CheckInAppointmentRequest, CheckInAppointmentResponse, AppointmentDTO>
{
  public override CheckInAppointmentResponse FromEntity(AppointmentDTO e)
    => new(new AppointmentRecord(
      e.Id,
      e.AppointmentNumber,
      e.CustomerName,
      e.AppointmentDate,
      e.EstimatedDuration,
      e.StatusName,
      e.TotalAmount
    ));
}