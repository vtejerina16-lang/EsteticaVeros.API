using EsteticaVeros.API.Core.ServiceCategoryAggregate;
using EsteticaVeros.API.Core.ServiceCategoryAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.ServiceCategories.Delete;

public class DeleteServiceCategoryHandler(
  IRepository<ServiceCategory> repository,
  ILogger<DeleteServiceCategoryHandler> logger) 
  : ICommandHandler<DeleteServiceCategoryCommand, Result>
{
  public async ValueTask<Result> Handle(
    DeleteServiceCategoryCommand command,
    CancellationToken cancellationToken)
  {
    var spec = new ServiceCategoryByIdSpec(command.CategoryId);
    var category = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (category == null)
    {
      return Result.NotFound();
    }

    // Soft delete
    category.Deactivate();
    await repository.UpdateAsync(category, cancellationToken);

    logger.LogInformation(
      "Service category deactivated: {CategoryNumber} - {Name}",
      category.CategoryNumber,
      category.Name);

    return Result.Success();
  }
}