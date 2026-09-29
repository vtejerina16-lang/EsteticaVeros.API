using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.PaymentMethods.Create;

public class CreatePaymentMethodHandler(IRepository<PaymentMethod> repository)
  : ICommandHandler<CreatePaymentMethodCommand, Result<int>>
{
  public async ValueTask<Result<int>> Handle(
    CreatePaymentMethodCommand command,
    CancellationToken cancellationToken)
  {
    // Validar que el tipo de método de pago sea válido
    if (!PaymentMethodType.TryFromValue(command.Type, out var paymentType))
    {
      return Result.Error("Invalid payment method type");
    }

    var paymentMethod = new PaymentMethod(
      command.Name,
      paymentType,
      command.RequiresCashRegister
    );

    var created = await repository.AddAsync(paymentMethod, cancellationToken);
    await repository.SaveChangesAsync(cancellationToken);

    return Result.Success(created.Id);
  }
}