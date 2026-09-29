using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.Payments.Cancel;

public class CancelPaymentHandler(IRepository<Payment> repository)
  : ICommandHandler<CancelPaymentCommand, Result>
{
  public async ValueTask<Result> Handle(
    CancelPaymentCommand command,
    CancellationToken cancellationToken)
  {
    var payment = await repository.GetByIdAsync(command.PaymentId, cancellationToken);

    if (payment == null)
    {
      return Result.NotFound("Payment not found");
    }

    try
    {
      payment.Cancel(command.Reason);

      await repository.UpdateAsync(payment, cancellationToken);
      await repository.SaveChangesAsync(cancellationToken);

      return Result.Success();
    }
    catch (InvalidOperationException ex)
    {
      return Result.Error(ex.Message);
    }
  }
}