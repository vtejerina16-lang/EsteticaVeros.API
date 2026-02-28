using EsteticaVeros.API.Core.ServiceAggregate;
using EsteticaVeros.API.Core.ServiceAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Services.Delete;

public class DeleteServiceHandler(
  IRepository<Service> repository,
  ILogger<DeleteServiceHandler> logger) 
  : ICommandHandler<DeleteServiceCommand, Result>
{
  public async ValueTask<Result> Handle(
    DeleteServiceCommand command,
    CancellationToken cancellationToken)
  {
    var spec = new ServiceByIdSpec(command.ServiceId);
    var service = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (service == null)
    {
      return Result.NotFound();
    }

    // Soft delete
    service.Deactivate();
    await repository.UpdateAsync(service, cancellationToken);

    logger.LogInformation(
      "Service deactivated: {ServiceNumber} - {Name}",
      service.ServiceNumber,
      service.Name);

    return Result.Success();
  }
}