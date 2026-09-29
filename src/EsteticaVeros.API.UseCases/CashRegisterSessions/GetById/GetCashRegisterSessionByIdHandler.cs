using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.CashRegisterSessions.GetById;

public class GetCashRegisterSessionByIdHandler(IReadRepository<CashRegisterSession> repository)
  : IQueryHandler<GetCashRegisterSessionByIdQuery, Result<CashRegisterSessionDTO>>
{
  public async ValueTask<Result<CashRegisterSessionDTO>> Handle(
    GetCashRegisterSessionByIdQuery query,
    CancellationToken cancellationToken)
  {
    var spec = new CashRegisterSessionByIdSpec(query.SessionId, includeCashRegister: true);
    var session = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (session == null)
    {
      return Result.NotFound();
    }

    var dto = new CashRegisterSessionDTO(
      session.Id,
      session.SessionNumber.Value,
      session.CashRegisterId,
      session.CashRegister.Name,
      session.OpenedByUserId,
      session.ClosedByUserId,
      session.OpenedAt,
      session.ClosedAt,
      session.InitialAmount,
      session.ExpectedAmount,
      session.ActualAmount,
      session.Difference,
      session.Status.Name,
      session.Status.DisplayName,
      session.Notes
    );

    return Result.Success(dto);
  }
}