using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.Update;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class Update(IMediator mediator)
  : Endpoint<UpdateAppointmentRequest,
             Results<Ok<UpdateAppointmentResponse>,
                     NotFound,
                     ProblemHttpResult>,
             UpdateAppointmentMapper>
{
  public override void Configure()
  {
    Put(UpdateAppointmentRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Actualizar cita";
      s.Description = "Actualiza los detalles de una cita existente, incluyendo fecha, servicios, asistente asignado y notas.";
      s.Responses[200] = "Cita actualizada exitosamente";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[400] = "Entrada inválida o cita no puede ser actualizada";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Administradores y Gerentes)";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<UpdateAppointmentResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdateAppointmentRequest request, CancellationToken ct)
  {
    var command = new UpdateAppointmentCommand(
      request.AppointmentId,
      request.AppointmentDate,
      request.ServiceIds,
      request.AssignedAssistantId,
      request.Notes,
      request.CustomerNotes,
      request.InternalNotes
    );

    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class UpdateAppointmentMapper
  : Mapper<UpdateAppointmentRequest, UpdateAppointmentResponse, AppointmentDTO>
{
  public override UpdateAppointmentResponse FromEntity(AppointmentDTO e)
    => new(new AppointmentDetailResponse(
      e.Id,
      e.AppointmentNumber,
      e.CustomerId,
      e.CustomerName,
      e.AssignedAssistantId,
      e.AssignedAssistantName,
      e.AppointmentDate,
      e.EstimatedDuration,
      e.ActualStartTime,
      e.ActualEndTime,
      e.ActualDuration,
      e.Status,
      e.StatusName,
      e.Source,
      e.SourceName,
      e.TotalAmount,
      e.DepositAmount,
      e.DepositPaid,
      e.PaymentStatus,
      e.PaymentStatusName,
      e.Notes,
      e.CustomerNotes,
      e.InternalNotes,
      e.CancellationReason,
      e.CancelledAt,
      e.ReminderSent,
      e.ReminderSentAt,
      e.ConfirmedByCustomer,
      e.ConfirmedAt,
      e.IsRecurring,
      e.RecurrencePattern,
      e.RecurrenceEndDate,
      e.CreatedAt,
      e.UpdatedAt,
      e.Services?.Select(s => new AppointmentServiceRecord(
        s.Id,
        s.ServiceId,
        s.ServiceName,
        s.Duration,
        s.Price,
        s.FinalPrice,
        s.StatusName
      )).ToList()
    ));
}