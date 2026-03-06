using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.Complete;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class Complete(IMediator mediator)
  : Endpoint<CompleteAppointmentRequest,
             Results<Ok<CompleteAppointmentResponse>,
                     NotFound,
                     ProblemHttpResult>,
             CompleteAppointmentMapper>
{
  public override void Configure()
  {
    Post(CompleteAppointmentRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Completar cita";
      s.Description = "Marca la cita y todos los servicios como completados. Actualiza las estadísticas del cliente.";
      s.Responses[200] = "Cita completada exitosamente";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[400] = "La cita no puede ser completada";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<CompleteAppointmentResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(CompleteAppointmentRequest request, CancellationToken ct)
  {
    var command = new CompleteAppointmentCommand(
      request.AppointmentId,
      request.Notes
    );

    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class CompleteAppointmentMapper
  : Mapper<CompleteAppointmentRequest, CompleteAppointmentResponse, AppointmentDTO>
{
  public override CompleteAppointmentResponse FromEntity(AppointmentDTO e)
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