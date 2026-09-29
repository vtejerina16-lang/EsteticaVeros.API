using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.CashRegisterSessions.List;

public class ListCashRegisterSessionsHandler(IReadRepository<CashRegisterSession> repository)
  : IQueryHandler<ListCashRegisterSessionsQuery, Result<IEnumerable<CashRegisterSessionDTO>>>
{
  public async ValueTask<Result<IEnumerable<CashRegisterSessionDTO>>> Handle(
    ListCashRegisterSessionsQuery query,
    CancellationToken cancellationToken)
  {
    IEnumerable<CashRegisterSession> sessions;

    // Si se especifica rango de fechas
    if (query.StartDate.HasValue && query.EndDate.HasValue)
    {
      var spec = new SessionsByDateRangeSpec(
        query.StartDate.Value,
        query.EndDate.Value,
        query.CashRegisterId,
        includeCashRegister: true
      );
      sessions = await repository.ListAsync(spec, cancellationToken);
    }
    // Si solo se especifica caja
    else if (query.CashRegisterId.HasValue)
    {
      var allSessions = await repository.ListAsync(cancellationToken);
      sessions = allSessions.Where(s => s.CashRegisterId == query.CashRegisterId.Value);
    }
    // Sin filtros
    else
    {
      sessions = await repository.ListAsync(cancellationToken);
    }

    // Filtrar por estado si se especifica
    if (!string.IsNullOrWhiteSpace(query.Status))
    {
      var statusEnum = SessionStatus.FromName(query.Status, ignoreCase: true);
      sessions = sessions.Where(s => s.Status == statusEnum);
    }

    var dtos = sessions.Select(s => new CashRegisterSessionDTO(
      s.Id,
      s.SessionNumber.Value,
      s.CashRegisterId,
      s.CashRegister?.Name ?? "N/A",
      s.OpenedByUserId,
      s.ClosedByUserId,
      s.OpenedAt,
      s.ClosedAt,
      s.InitialAmount,
      s.ExpectedAmount,
      s.ActualAmount,
      s.Difference,
      s.Status.Name,
      s.Status.DisplayName,
      s.Notes
    ));

    return Result.Success(dtos);
  }
}