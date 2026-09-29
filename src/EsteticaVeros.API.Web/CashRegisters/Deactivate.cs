using EsteticaVeros.API.UseCases.CashRegisters.Deactivate;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisters;

public class Deactivate(IMediator mediator)
  : Endpoint<DeactivateCashRegisterRequest,
             Results<NoContent, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(DeactivateCashRegisterRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Desactivar caja registradora";
      s.Description = "Desactiva una caja registradora activa";
      s.Responses[204] = "Caja registradora desactivada exitosamente";
      s.Responses[404] = "Caja registradora no encontrada";
      s.Responses[401] = "No autenticado";
      s.Responses[403] = "No autorizado (Solo Administrador o Gerente)";
    });

    Tags("CashRegisters");
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeactivateCashRegisterRequest req, CancellationToken ct)
  {
    var command = new DeactivateCashRegisterCommand(req.CashRegisterId);
    var result = await mediator.Send(command, ct);

    return result.ToDeleteResult();
  }
}