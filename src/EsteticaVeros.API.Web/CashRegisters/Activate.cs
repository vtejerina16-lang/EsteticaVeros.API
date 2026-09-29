using EsteticaVeros.API.UseCases.CashRegisters.Activate;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisters;

public class Activate(IMediator mediator)
  : Endpoint<ActivateCashRegisterRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(ActivateCashRegisterRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Activar caja registradora";
      s.Description = "Activa una caja registradora desactivada";
      s.Responses[204] = "Caja registradora activada exitosamente";
      s.Responses[404] = "Caja registradora no encontrada";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador o Gerente)";
    });

    Tags("CashRegisters");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(ActivateCashRegisterRequest req, CancellationToken ct)
  {
    var command = new ActivateCashRegisterCommand(req.CashRegisterId);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}