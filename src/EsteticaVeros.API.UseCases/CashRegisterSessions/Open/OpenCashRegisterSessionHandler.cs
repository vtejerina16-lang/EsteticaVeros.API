using EsteticaVeros.API.Core.CashRegisterAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.CashRegisterSessions.Open;

public class OpenCashRegisterSessionHandler(
  IRepository<CashRegisterSession> sessionRepository,
  IReadRepository<CashRegister> cashRegisterRepository)
  : ICommandHandler<OpenCashRegisterSessionCommand, Result<int>>
{
  public async ValueTask<Result<int>> Handle(
    OpenCashRegisterSessionCommand command,
    CancellationToken cancellationToken)
  {
    // Verificar que la caja existe y está activa
    var cashRegister = await cashRegisterRepository.GetByIdAsync(command.CashRegisterId, cancellationToken);
    if (cashRegister == null)
    {
      return Result.NotFound("Cash register not found");
    }

    if (!cashRegister.IsActive)
    {
      return Result.Error("Cash register is not active");
    }

    // Verificar que no hay otra sesión abierta para esta caja
    var openSessionsSpec = new OpenSessionsSpec(command.CashRegisterId);
    var openSessions = await sessionRepository.ListAsync(openSessionsSpec, cancellationToken);

    if (openSessions.Any())
    {
      return Result.Error("There is already an open session for this cash register");
    }

    // Generar número de sesión
    var year = DateTime.UtcNow.Year;
    var allSessions = await sessionRepository.ListAsync(cancellationToken);
    var sessionsThisYear = allSessions.Count(s => s.OpenedAt.Year == year);
    var sessionNumber = SessionNumber.Generate(year, sessionsThisYear + 1);

    // Crear nueva sesión
    var session = new CashRegisterSession(
      sessionNumber,
      command.CashRegisterId,
      command.InitialAmount,
      command.OpenedByUserId
    );

    var createdSession = await sessionRepository.AddAsync(session, cancellationToken);
    await sessionRepository.SaveChangesAsync(cancellationToken);

    return Result.Success(createdSession.Id);
  }
}