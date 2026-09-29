using EsteticaVeros.API.UseCases.CashMovements.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashMovements;

public class Create(IMediator mediator)
  : Endpoint<CreateCashMovementRequest,
             Results<Created<CreateCashMovementResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreateCashMovementMapper>
{
  public override void Configure()
  {
    Post(CreateCashMovementRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Crea un movimiento de caja";
      s.Description = "Crea un movimiento de caja manual (ingreso/gasto/retiro/deposito). Type: 1=Ingreso, 2=Gasto, 3=Retiro, 4=Deposito, 5=Transferencia, 6=Ajuste";
      s.ExampleRequest = new CreateCashMovementRequest
      {
        CashRegisterSessionId = 1,
        Type = 2,
        Amount = 50.00m,
        Description = "Compra de insumos de limpieza",
        Reference = "Factura #123"
      };
      s.Responses[201] = "Movimiento de caja creado exitosamente";
      s.Responses[400] = "Entrada inválida o sesión no abierta";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (solo Admininistrador o Gerente)";
    });

    Tags("CashMovements");
  }

  public override async Task<Results<Created<CreateCashMovementResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateCashMovementRequest req, CancellationToken ct)
  {
    var userId = User.GetUserId();

    var command = new CreateCashMovementCommand(
      req.CashRegisterSessionId,
      req.Type,
      req.Amount,
      req.Description,
      req.Reference,
      userId
    );

    var result = await mediator.Send(command, ct);

    return result.ToCreatedResult(
      id => CreateCashMovementRequest.BuildRoute(id),
      id => new CreateCashMovementResponse(id)
    );
  }
}

public sealed class CreateCashMovementMapper
  : Mapper<CreateCashMovementRequest, CreateCashMovementResponse, int>
{
  public override CreateCashMovementResponse FromEntity(int id)
    => new(id);
}