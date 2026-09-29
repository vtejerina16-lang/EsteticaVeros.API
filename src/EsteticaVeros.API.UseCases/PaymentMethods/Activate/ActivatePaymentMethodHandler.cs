using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.PaymentMethods.Activate;

public class ActivatePaymentMethodHandler(IRepository<PaymentMethod> repository)
  : ICommandHandler<ActivatePaymentMethodCommand, Result>
{
  public async ValueTask<Result> Handle(
    ActivatePaymentMethodCommand command,
    CancellationToken cancellationToken)
  {
    var paymentMethod = await repository.GetByIdAsync(command.PaymentMethodId, cancellationToken);

    if (paymentMethod == null)
    {
      return Result.NotFound();
    }

    paymentMethod.Activate();

    await repository.UpdateAsync(paymentMethod, cancellationToken);
    await repository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}