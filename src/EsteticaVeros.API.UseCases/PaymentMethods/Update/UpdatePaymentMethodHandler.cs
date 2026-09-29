using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.PaymentMethods.Update;

public class UpdatePaymentMethodHandler(IRepository<PaymentMethod> repository)
  : ICommandHandler<UpdatePaymentMethodCommand, Result<PaymentMethodDTO>>
{
  public async ValueTask<Result<PaymentMethodDTO>> Handle(
    UpdatePaymentMethodCommand command,
    CancellationToken cancellationToken)
  {
    var paymentMethod = await repository.GetByIdAsync(command.PaymentMethodId, cancellationToken);

    if (paymentMethod == null)
    {
      return Result.NotFound();
    }

    // Validar tipo
    if (!PaymentMethodType.TryFromValue(command.Type, out var paymentType))
    {
      return Result.Error("Invalid payment method type");
    }

    paymentMethod.UpdateInfo(command.Name, paymentType, command.RequiresCashRegister);

    await repository.UpdateAsync(paymentMethod, cancellationToken);
    await repository.SaveChangesAsync(cancellationToken);

    var dto = new PaymentMethodDTO(
      paymentMethod.Id,
      paymentMethod.Name,
      paymentMethod.Type.Value,
      paymentMethod.Type.DisplayName,
      paymentMethod.RequiresCashRegister,
      paymentMethod.IsActive,
      paymentMethod.CreatedAt,
      paymentMethod.UpdatedAt
    );

    return Result.Success(dto);
  }
}