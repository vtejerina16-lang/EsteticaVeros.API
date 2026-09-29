using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Customers.Delete;

public class DeleteCustomerHandler(
  IRepository<Customer> repository,
  ILogger<DeleteCustomerHandler> logger) : ICommandHandler<DeleteCustomerCommand, Result>
{
  public async ValueTask<Result> Handle(
    DeleteCustomerCommand command,
    CancellationToken cancellationToken)
  {
    var spec = new CustomerByIdSpec(command.CustomerId);
    var customer = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (customer == null)
    {
      return Result.NotFound();
    }

    // Soft delete
    customer.Deactivate();
    await repository.UpdateAsync(customer, cancellationToken);

    logger.LogInformation(
      "Customer deactivated: {CustomerNumber} - {FullName}",
      customer.CustomerNumber,
      customer.FullName);

    return Result.Success();
  }
}