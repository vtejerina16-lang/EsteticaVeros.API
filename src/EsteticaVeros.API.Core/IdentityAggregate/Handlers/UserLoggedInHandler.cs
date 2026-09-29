using EsteticaVeros.API.Core.IdentityAggregate.Events;

namespace EsteticaVeros.API.Core.IdentityAggregate.Handlers;

public class UserLoggedInHandler(
  ILogger<UserLoggedInHandler> logger) : INotificationHandler<UserLoggedInEvent>
{
  public async ValueTask Handle(UserLoggedInEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling User Logged In event for user {userId} - {email} at {loginTime}", 
      domainEvent.UserId, 
      domainEvent.Email,
      domainEvent.LoginTime);

    // TODO: Aquí puedes:
    // - Registrar en tabla de auditoría
    // - Actualizar estadísticas de uso
    // - Enviar notificaciones si es necesario
    
    await Task.CompletedTask;
  }
}
