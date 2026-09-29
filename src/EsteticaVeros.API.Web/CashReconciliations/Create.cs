using EsteticaVeros.API.UseCases.CashReconciliations.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashReconciliations;

public class Create(IMediator mediator)
  : Endpoint<CreateCashReconciliationRequest,
             Results<Created<CreateCashReconciliationResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreateCashReconciliationMapper>
{
  public override void Configure()
  {
    Post(CreateCashReconciliationRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Crea un arqueo de caja";
      s.Description = "Crea un arqueo de caja para una sesión de caja cerrada. El arqueo incluye el monto contado, detalles de denominación y notas opcionales.";
      s.ExampleRequest = new CreateCashReconciliationRequest
      {
        CashRegisterSessionId = 1,
        CountedAmount = 2295.00m,
        DenominationDetails = "{\"200Bs\":10,\"100Bs\":2,\"50Bs\":1,\"20Bs\":2,\"5Bs\":1}",
        Notes = "Arqueo de turno de mañana"
      };
      s.Responses[201] = "Arqueo creado exitosamente";
      s.Responses[400] = "Entrada inválida o sesión no cerrada";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador o Gerente)";
    });

    Tags("CashReconciliations");
  }

  public override async Task<Results<Created<CreateCashReconciliationResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateCashReconciliationRequest req, CancellationToken ct)
  {
    var userId = User.GetUserId();
    if (!userId.HasValue)
    {
      return TypedResults.Problem(
        title: "Authentication required",
        detail: "User ID is required to create reconciliation",
        statusCode: 401
      );
    }

    var command = new CreateCashReconciliationCommand(
      req.CashRegisterSessionId,
      req.CountedAmount,
      userId.Value,
      req.DenominationDetails,
      req.Notes
    );

    var result = await mediator.Send(command, ct);

    return result.ToCreatedResult(
      id => CreateCashReconciliationRequest.BuildRoute(id),
      id => new CreateCashReconciliationResponse(id)
    );
  }
}

public sealed class CreateCashReconciliationMapper
  : Mapper<CreateCashReconciliationRequest, CreateCashReconciliationResponse, int>
{
  public override CreateCashReconciliationResponse FromEntity(int id)
    => new(id);
}