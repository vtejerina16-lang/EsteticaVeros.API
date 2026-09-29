using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.PaymentMethods.GetById;

public class GetPaymentMethodByIdHandler(IReadRepository<PaymentMethod> repository)
  : IQueryHandler<GetPaymentMethodByIdQuery, Result<PaymentMethodDTO>>
{
  public async ValueTask<Result<PaymentMethodDTO>> Handle(
    GetPaymentMethodByIdQuery query,
    CancellationToken cancellationToken)
  {
    var paymentMethod = await repository.GetByIdAsync(query.PaymentMethodId, cancellationToken);

    if (paymentMethod == null)
    {
      return Result.NotFound();
    }

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