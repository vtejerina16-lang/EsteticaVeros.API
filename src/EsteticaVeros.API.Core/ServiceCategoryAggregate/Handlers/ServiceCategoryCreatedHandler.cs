using EsteticaVeros.API.Core.ServiceCategoryAggregate.Events;

namespace EsteticaVeros.API.Core.ServiceCategoryAggregate.Handlers;

public class ServiceCategoryCreatedHandler(ILogger<ServiceCategoryCreatedHandler> logger)
  : INotificationHandler<ServiceCategoryCreatedEvent>
{
  public ValueTask Handle(ServiceCategoryCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation(
      "New service category created: {CategoryNumber} - {Name}",
      domainEvent.Category.CategoryNumber,
      domainEvent.Category.Name);

    return ValueTask.CompletedTask;
  }
}