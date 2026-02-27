using EsteticaVeros.API.Core.ServiceCategoryAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Handlers;

public class ServiceCategoryUpdatedHandler(ILogger<ServiceCategoryUpdatedHandler> logger)
  : INotificationHandler<ServiceCategoryUpdatedEvent>
{
  public ValueTask Handle(ServiceCategoryUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "Service category updated: {CategoryNumber} - {Name}",
      domainEvent.Category.CategoryNumber,
      domainEvent.Category.Name);

    return ValueTask.CompletedTask;
  }
}