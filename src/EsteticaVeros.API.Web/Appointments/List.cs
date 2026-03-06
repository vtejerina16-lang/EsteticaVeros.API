using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.List;
using EsteticaVeros.API.Web.Configurations;

namespace EsteticaVeros.API.Web.Appointments;

public class List(IMediator mediator)
  : Endpoint<ListAppointmentsRequest, AppointmentsListResponse, ListAppointmentsMapper>
{
  public override void Configure()
  {
    Get("/api/appointments");
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Lista de Citas";
      s.Description = "Obtiene una lista de citas con opciones de filtrado por fecha, cliente, asistente asignado, estado y fuente.";
      
      s.Params["start_date"] = "Filter by start date (ISO 8601)";
      s.Params["end_date"] = "Filter by end date (ISO 8601)";
      s.Params["customer_id"] = "Filter by customer ID";
      s.Params["assigned_assistant_id"] = "Filter by assigned assistant ID";
      s.Params["status"] = "Filter by status (1=Scheduled, 2=Confirmed, etc.)";
      s.Params["source"] = "Filter by source (1=InOffice, 2=WhatsApp, etc.)";

      s.Responses[200] = "Lista de citas obtenida exitosamente";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");
  }

  public override async Task HandleAsync(ListAppointmentsRequest req, CancellationToken ct)
  {
    var query = new ListAppointmentsQuery(
      req.StartDate,
      req.EndDate,
      req.CustomerId,
      req.AssignedAssistantId,
      req.Status,
      req.Source
    );

    var result = await mediator.Send(query, ct);

    if (!result.IsSuccess)
    {
      await Send.ErrorsAsync(statusCode: 400, ct);
      return;
    }

    var response = Map.FromEntity(result.Value);
    await Send.OkAsync(response, ct);
  }
}