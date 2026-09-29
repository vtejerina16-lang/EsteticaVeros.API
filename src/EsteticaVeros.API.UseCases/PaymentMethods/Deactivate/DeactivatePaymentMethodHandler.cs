using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.PaymentMethods.Deactivate;

public class DeactivatePaymentMethodHandler(IRepository<PaymentMethod> repository)
  : ICommandHandler<DeactivatePaymentMethodCommand, Result>
{
  public async ValueTask<Result> Handle(
    DeactivatePaymentMethodCommand command,
    CancellationToken cancellationToken)
  {
    var paymentMethod = await repository.GetByIdAsync(command.PaymentMethodId, cancellationToken);

    if (paymentMethod == null)
    {
      return Result.NotFound();
    }

    paymentMethod.Deactivate();

    await repository.UpdateAsync(paymentMethod, cancellationToken);
    await repository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}