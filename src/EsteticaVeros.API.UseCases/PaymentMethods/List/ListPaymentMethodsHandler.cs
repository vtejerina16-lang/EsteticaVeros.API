using EsteticaVeros.API.Core.PaymentAggregate;

namespace EsteticaVeros.API.UseCases.PaymentMethods.List;

public class ListPaymentMethodsHandler(IReadRepository<PaymentMethod> repository)
  : IQueryHandler<ListPaymentMethodsQuery, Result<IEnumerable<PaymentMethodDTO>>>
{
  public async ValueTask<Result<IEnumerable<PaymentMethodDTO>>> Handle(
    ListPaymentMethodsQuery query,
    CancellationToken cancellationToken)
  {
    var paymentMethods = await repository.ListAsync(cancellationToken);

    // Filtrar por IsActive
    if (query.IsActive.HasValue)
    {
      paymentMethods = paymentMethods.Where(p => p.IsActive == query.IsActive.Value).ToList();
    }

    // Filtrar por RequiresCashRegister
    if (query.RequiresCashRegister.HasValue)
    {
      paymentMethods = paymentMethods
        .Where(p => p.RequiresCashRegister == query.RequiresCashRegister.Value)
        .ToList();
    }

    var dtos = paymentMethods.Select(p => new PaymentMethodDTO(
      p.Id,
      p.Name,
      p.Type.Value,
      p.Type.DisplayName,
      p.RequiresCashRegister,
      p.IsActive,
      p.CreatedAt,
      p.UpdatedAt
    ));

    return Result.Success(dtos);
  }
}