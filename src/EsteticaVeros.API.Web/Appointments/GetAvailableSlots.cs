using EsteticaVeros.API.UseCases.Appointments.GetAvailableSlots;
using EsteticaVeros.API.Web.Configurations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class GetAvailableSlots(IMediator mediator)
  : Endpoint<GetAvailableSlotsRequest,
             Results<Ok<AvailableSlotsResponse>,
                     ProblemHttpResult>,
             GetAvailableSlotsMapper>
{
  public override void Configure()
  {
    Get(GetAvailableSlotsRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Lista de espacios disponibles";
      s.Description = "Devuelve los espacios de tiempo disponibles para una fecha específica basados en horas de trabajo y citas existentes.";

      s.Params["date"] = "Date to check availability (ISO 8601 format)";
      s.Params["estimated_duration"] = "Estimated duration in minutes";
      s.Params["assigned_assistant_id"] = "Optional: Filter by specific assistant";

      s.Responses[200] = "Lista de espacios disponibles obtenida exitosamente";
      s.Responses[400] = "Parámetros inválidos";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task<Results<Ok<AvailableSlotsResponse>, ProblemHttpResult>>
    ExecuteAsync(GetAvailableSlotsRequest request, CancellationToken ct)
  {
    var query = new GetAvailableSlotsQuery(
      request.Date,
      request.EstimatedDuration,
      request.AssignedAssistantId
    );

    var result = await mediator.Send(query, ct);

    if (!result.IsSuccess)
    {
      return TypedResults.Problem(
        title: "Failed to get available slots",
        detail: result.Errors.FirstOrDefault(),
        statusCode: 400
      );
    }

    var response = Map.FromEntity(result.Value);
    return TypedResults.Ok(response);
  }
}

public sealed class GetAvailableSlotsMapper
  : Mapper<GetAvailableSlotsRequest, AvailableSlotsResponse, List<TimeSlotDTO>>
{
  public override AvailableSlotsResponse FromEntity(List<TimeSlotDTO> e)
  {
    var slots = e.Select(slot => new TimeSlotRecord(
      slot.StartTime,
      slot.EndTime,
      slot.IsAvailable,
      slot.UnavailableReason
    )).ToList();

    return new AvailableSlotsResponse(slots);
  }
}