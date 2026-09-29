using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.MarkNoShow;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class MarkNoShow(IMediator mediator)
  : Endpoint<MarkNoShowRequest,
             Results<Ok<MarkNoShowResponse>,
                     NotFound,
                     ProblemHttpResult>,
             MarkNoShowMapper>
{
  public override void Configure()
  {
    Post(MarkNoShowRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Marcar NoShow";
      s.Description = "Marca una cita como NoShow, aplicando penalización si está configurada.";
      s.Responses[200] = "Cita marcada como NoShow exitosamente";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[400] = "La cita no puede ser marcada como NoShow";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administradores y Gerentes)";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<MarkNoShowResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(MarkNoShowRequest request, CancellationToken ct)
  {
    var command = new MarkNoShowCommand(
      request.AppointmentId,
      request.Reason
    );

    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class MarkNoShowMapper
  : Mapper<MarkNoShowRequest, MarkNoShowResponse, AppointmentDTO>
{
  public override MarkNoShowResponse FromEntity(AppointmentDTO e)
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