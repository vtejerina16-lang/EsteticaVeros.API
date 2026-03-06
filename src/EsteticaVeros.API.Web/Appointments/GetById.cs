using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.Get;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class GetById(IMediator mediator)
  : Endpoint<GetAppointmentByIdRequest,
             Results<Ok<AppointmentDetailResponse>,
                     NotFound,
                     ProblemHttpResult>,
             GetAppointmentByIdMapper>
{
  public override void Configure()
  {
    Get(GetAppointmentByIdRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Obtener cita por ID";
      s.Description = "Obtiene los detalles de una cita específica utilizando su ID.";
      s.Responses[200] = "Cita encontrada";
      s.Responses[404] = "Cita no encontrada";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<AppointmentDetailResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetAppointmentByIdRequest request, CancellationToken ct)
  {
    var result = await mediator.Send(new GetAppointmentQuery(request.AppointmentId), ct);
    return result.ToGetByIdResult(Map.FromEntity);
  }
}

public sealed class GetAppointmentByIdMapper
  : Mapper<GetAppointmentByIdRequest, AppointmentDetailResponse, AppointmentDTO>
{
  public override AppointmentDetailResponse FromEntity(AppointmentDTO e)
    => new(
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
    );
}