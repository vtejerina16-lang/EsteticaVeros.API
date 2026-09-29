using EsteticaVeros.API.Core.ServiceCategoryAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Handlers;

public class ServiceCategoryDeactivatedHandler(ILogger<ServiceCategoryDeactivatedHandler> logger)
  : INotificationHandler<ServiceCategoryDeactivatedEvent>
{
  public ValueTask Handle(ServiceCategoryDeactivatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Service category deactivated: {CategoryNumber} - {Name}",
      domainEvent.Category.CategoryNumber,
      domainEvent.Category.Name);

    return ValueTask.CompletedTask;
  }
}