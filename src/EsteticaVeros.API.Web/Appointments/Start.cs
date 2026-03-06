using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.Start;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class Start(IMediator mediator)
  : Endpoint<StartAppointmentRequest,
             Results<Ok<StartAppointmentResponse>,
                     NotFound,
                     ProblemHttpResult>,
             StartAppointmentMapper>
{
  public override void Configure()
  {
    Post(StartAppointmentRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Iniciar cita";
      s.Description = "Marca la cita como en progreso y comienza el primer servicio.";
      s.Responses[200] = "Cita iniciada exitosamente";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[400] = "La cita no puede ser iniciada";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<StartAppointmentResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(StartAppointmentRequest request, CancellationToken ct)
  {
    var command = new StartAppointmentCommand(request.AppointmentId);
    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class StartAppointmentMapper
  : Mapper<StartAppointmentRequest, StartAppointmentResponse, AppointmentDTO>
{
  public override StartAppointmentResponse FromEntity(AppointmentDTO e)
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