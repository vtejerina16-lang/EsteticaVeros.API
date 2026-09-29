using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.CashRegisterSessions.Close;

public class CloseCashRegisterSessionHandler(IRepository<CashRegisterSession> repository)
  : ICommandHandler<CloseCashRegisterSessionCommand, Result>
{
  public async ValueTask<Result> Handle(
    CloseCashRegisterSessionCommand command,
    CancellationToken cancellationToken)
  {
    var session = await repository.GetByIdAsync(command.SessionId, cancellationToken);

    if (session == null)
    {
      return Result.NotFound("Session not found");
    }

    if (!session.IsOpen)
    {
      return Result.Error($"Session is already {session.Status.DisplayName}");
    }

    try
    {
      session.Close(command.ClosedByUserId, command.Notes);

      await repository.UpdateAsync(session, cancellationToken);
      await repository.SaveChangesAsync(cancellationToken);

      return Result.Success();
    }
    catch (InvalidOperationException ex)
    {
      return Result.Error(ex.Message);
    }
  }
}