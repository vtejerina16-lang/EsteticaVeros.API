using EsteticaVeros.API.UseCases.CashRegisters;
using EsteticaVeros.API.UseCases.CashRegisters.Create;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisters;

public class Create(IMediator mediator)
  : Endpoint<CreateCashRegisterRequest,
             Results<Created<CreateCashRegisterResponse>,
                     ValidationProblem,
                     ProblemHttpResult>,
             CreateCashRegisterMapper>
{
  public override void Configure()
  {
    Post(CreateCashRegisterRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Crear caja registradora";
      s.Description = "Crea una nueva caja registradora en el sistema";
      s.ExampleRequest = new CreateCashRegisterRequest
      {
        Name = "Caja Principal",
        Location = "Recepción"
      };
      s.Responses[201] = "Caja registradora creada exitosamente";
      s.Responses[400] = "Entrada inválida";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador o Gerente)";
    });

    Tags("CashRegisters");
  }

  public override async Task<Results<Created<CreateCashRegisterResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateCashRegisterRequest req, CancellationToken ct)
  {
    var command = new CreateCashRegisterCommand(req.Name, req.Location);
    var result = await mediator.Send(command, ct);

    return result.ToCreatedResult(
      id => CreateCashRegisterRequest.BuildRoute(id),
      id => new CreateCashRegisterResponse(id)
    );
  }
}

public sealed class CreateCashRegisterMapper
  : Mapper<CreateCashRegisterRequest, CreateCashRegisterResponse, int>
{
  public override CreateCashRegisterResponse FromEntity(int id)
    => new(id);
}