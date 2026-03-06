using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.Cancel;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class Cancel(IMediator mediator)
  : Endpoint<CancelAppointmentRequest,
             Results<Ok<CancelAppointmentResponse>,
                     NotFound,
                     ProblemHttpResult>,
             CancelAppointmentMapper>
{
  public override void Configure()
  {
    Post(CancelAppointmentRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Cancelar una cita";
      s.Description = "Permite cancelar una cita existente proporcionando el motivo de la cancelación.";
      s.Responses[200] = "Cita cancelada exitosamente";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[400] = "La cita no puede ser cancelada";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<CancelAppointmentResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(CancelAppointmentRequest request, CancellationToken ct)
  {
    var command = new CancelAppointmentCommand(
      request.AppointmentId,
      request.CancellationReason,
      User.GetUserId()
    );

    var result = await mediator.Send(command, ct);
    return result.ToUpdateResult(Map.FromEntity);
  }
}

public sealed class CancelAppointmentMapper
  : Mapper<CancelAppointmentRequest, CancelAppointmentResponse, AppointmentDTO>
{
  public override CancelAppointmentResponse FromEntity(AppointmentDTO e)
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