using EsteticaVeros.API.UseCases.CashRegisterSessions.Open;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisterSessions;

public class Open(IMediator mediator)
  : Endpoint<OpenCashRegisterSessionRequest,
             Results<Created<OpenCashRegisterSessionResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             OpenCashRegisterSessionMapper>
{
  public override void Configure()
  {
    Post(OpenCashRegisterSessionRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Abrir sesión de caja registradora";
      s.Description = "Abre una nueva sesión de caja registradora para una caja específica";
      s.ExampleRequest = new OpenCashRegisterSessionRequest
      {
        CashRegisterId = 1,
        InitialAmount = 500.00m
      };
      s.Responses[201] = "Sesión abierta exitosamente";
      s.Responses[400] = "Entrada inválida o sesión ya está abierta";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador o Gerente)";
    });

    Tags("CashRegisterSessions");
  }

  public override async Task<Results<Created<OpenCashRegisterSessionResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(OpenCashRegisterSessionRequest req, CancellationToken ct)
  {
    var userId = User.GetUserId();
    if (!userId.HasValue)
    {
      return TypedResults.Problem(
        title: "Authentication required",
        detail: "User ID is required to open a session",
        statusCode: 401
      );
    }
    
    var command = new OpenCashRegisterSessionCommand(
      req.CashRegisterId,
      req.InitialAmount,
      userId.Value
    );

    var result = await mediator.Send(command, ct);

    return result.ToCreatedResult(
      id => OpenCashRegisterSessionRequest.BuildRoute(id),
      id => new OpenCashRegisterSessionResponse(id)
    );
  }
}

public sealed class OpenCashRegisterSessionMapper
  : Mapper<OpenCashRegisterSessionRequest, OpenCashRegisterSessionResponse, int>
{
  public override OpenCashRegisterSessionResponse FromEntity(int id)
    => new(id);
}