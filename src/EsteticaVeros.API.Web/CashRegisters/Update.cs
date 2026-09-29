using EsteticaVeros.API.UseCases.CashRegisters;
using EsteticaVeros.API.UseCases.CashRegisters.Update;
using EsteticaVeros.API.Web.Configurations;
using EsteticaVeros.API.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EsteticaVeros.API.Web.CashRegisters;

public class Update(IMediator mediator)
  : Endpoint<UpdateCashRegisterRequest,
             Results<Ok<CashRegisterDTO>, NotFound, ProblemHttpResult>>
{
  public override void Configure()
  {
    Put(UpdateCashRegisterRequest.Route);
    Policies(AuthorizationConfig.RequireAdminOrManager);

    Summary(s =>
    {
      s.Summary = "Actualizar caja registradora";
      s.Description = "Actualiza la información de una caja registradora específica";
      s.Responses[200] = "Caja registradora actualizada exitosamente";
      s.Responses[404] = "Caja registradora no encontrada";
      s.Responses[400] = "Entrada inválida";
      s.Responses[401] = "No autenticado  ";
      s.Responses[403] = "No autorizado (solo Administrador o Gerente)";
    });

    Tags("CashRegisters");
  }

  public override async Task<Results<Ok<CashRegisterDTO>, NotFound, ProblemHttpResult>>
    ExecuteAsync(UpdateCashRegisterRequest req, CancellationToken ct)
  {
    var command = new UpdateCashRegisterCommand(req.CashRegisterId, req.Name, req.Location);
    var result = await mediator.Send(command, ct);

    return result.ToUpdateResult(dto => dto);
  }
}