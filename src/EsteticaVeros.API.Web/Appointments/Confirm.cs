using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.Confirm;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class Confirm(IMediator mediator)
  : Endpoint<ConfirmAppointmentRequest,
             Results<Ok<ConfirmAppointmentResponse>,
                     NotFound,
                     ProblemHttpResult>,
             ConfirmAppointmentMapper>
{
  public override void Configure()
  {
    Post(ConfirmAppointmentRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Confirmar cita";
      s.Description = "Marca una cita como confirmada por el cliente.";
      s.Responses[200] = "Cita confirmada exitosamente";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[400] = "La cita no puede ser confirmada";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<ConfirmAppointmentResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(ConfirmAppointmentRequest request, CancellationToken ct)
  {
    var command = new ConfirmAppointmentCommand(request.AppointmentId);
    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class ConfirmAppointmentMapper
  : Mapper<ConfirmAppointmentRequest, ConfirmAppointmentResponse, AppointmentDTO>
{
  public override ConfirmAppointmentResponse FromEntity(AppointmentDTO e)
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