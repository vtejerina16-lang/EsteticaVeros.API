using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.Reschedule;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class Reschedule(IMediator mediator)
  : Endpoint<RescheduleAppointmentRequest,
             Results<Ok<RescheduleAppointmentResponse>,
                     NotFound,
                     ProblemHttpResult>,
             RescheduleAppointmentMapper>
{
  public override void Configure()
  {
    Post(RescheduleAppointmentRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Reprogramar cita";
      s.Description = "Cambia la cita a una nueva fecha/hora. Crea una nueva cita y marca la antigua como reprogramada.";
      s.Responses[200] = "Cita reprogramada exitosamente";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[400] = "El nuevo espacio de tiempo no está disponible o la cita no puede ser reprogramada";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<RescheduleAppointmentResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(RescheduleAppointmentRequest request, CancellationToken ct)
  {
    var command = new RescheduleAppointmentCommand(
      request.AppointmentId,
      request.NewAppointmentDate,
      request.Reason
    );

    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class RescheduleAppointmentMapper
  : Mapper<RescheduleAppointmentRequest, RescheduleAppointmentResponse, AppointmentDTO>
{
  public override RescheduleAppointmentResponse FromEntity(AppointmentDTO e)
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