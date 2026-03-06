using EsteticaVeros.API.UseCases.Appointments;
using EsteticaVeros.API.UseCases.Appointments.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.Appointments;

public class Create(IMediator mediator)
  : Endpoint<CreateAppointmentRequest,
             Results<Created<CreateAppointmentResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreateAppointmentMapper>
{
  public override void Configure()
  {
    Post(CreateAppointmentRequest.Route);
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Crear una cita";
      s.Description = "Crea una nueva cita para un cliente, asignando servicios, asistente y detalles adicionales.";
      s.ExampleRequest = new CreateAppointmentRequest
      {
        CustomerId = 1,
        AppointmentDate = DateTime.UtcNow.AddDays(7),
        Source = 1,
        ServiceIds = new List<int> { 1, 2 },
        AssignedAssistantId = 1,
        Notes = "Cliente prefiere tarde",
        CustomerNotes = "Es la primera vez que visito el salón",
        InternalNotes = "Revisar historial de cliente antes de la cita",
        IsRecurring = false
      };
      // s.ExampleResponse = new CreateAppointmentResponse(new AppointmentRecord(
      //   Id: 456,
      //   AppointmentNumber: "APT-20240601-0001",
      //   CustomerName: "Juan Pérez",
      //   AppointmentDate: DateTime.UtcNow.AddDays(7),
      //   EstimatedDuration: TimeSpan.FromHours(2),
      //   StatusName: "Programada",
      //   TotalAmount: 150.00m
      // ));

      s.Responses[201] = "Cita creada exitosamente";
      s.Responses[400] = "Error de validación o franja horaria no disponible";
      s.Responses[401] = "No autenticado";
    });

    Tags("Appointments");

    Description(builder => builder
      .Accepts<CreateAppointmentRequest>("application/json")
      .Produces<CreateAppointmentResponse>(201, "application/json")
      .ProducesProblem(400));
  }

  public override async Task<Results<Created<CreateAppointmentResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateAppointmentRequest req, CancellationToken ct)
  {
    var command = new CreateAppointmentCommand(
      req.CustomerId,
      req.AppointmentDate,
      req.Source,
      req.ServiceIds,
      req.AssignedAssistantId,
      req.Notes,
      req.CustomerNotes,
      req.InternalNotes,
      req.IsRecurring,
      req.RecurrencePattern,
      req.RecurrenceEndDate,
      User.GetUserId()
    );

    var result = await mediator.Send(command, ct);

    if (!result.IsSuccess)
    {
      return TypedResults.Problem(
        title: "Appointment creation failed",
        detail: result.Errors.FirstOrDefault(),
        statusCode: 400
      );
    }

    var response = Map.FromEntity(result.Value);
    return TypedResults.Created($"/api/appointments/{result.Value.Id}", response);
  }
}

public sealed class CreateAppointmentMapper
  : Mapper<CreateAppointmentRequest, CreateAppointmentResponse, AppointmentDTO>
{
  public override CreateAppointmentResponse FromEntity(AppointmentDTO e)
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