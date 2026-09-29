using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.CashRegisterSessions.GetOpenSession;

public class GetOpenSessionHandler(IReadRepository<CashRegisterSession> repository)
  : IQueryHandler<GetOpenSessionQuery, Result<CashRegisterSessionDTO?>>
{
  public async ValueTask<Result<CashRegisterSessionDTO?>> Handle(
    GetOpenSessionQuery query,
    CancellationToken cancellationToken)
  {
    var spec = new OpenSessionsSpec(query.CashRegisterId, includeCashRegister: true);
    var sessions = await repository.ListAsync(spec, cancellationToken);
    var session = sessions.FirstOrDefault();

    if (session == null)
    {
      return Result.Success<CashRegisterSessionDTO?>(null);
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

    return Result.Success<CashRegisterSessionDTO?>(dto);
  }
}