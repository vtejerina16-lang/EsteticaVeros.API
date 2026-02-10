using EsteticaVeros.API.Core.IdentityAggregate.Events;
using EsteticaVeros.API.Core.Interfaces;

namespace EsteticaVeros.API.Core.IdentityAggregate.Handlers;

public class UserCreatedEmailNotificationHandler(
  ILogger<UserCreatedEmailNotificationHandler> logger,
  IEmailSender emailSender) : INotificationHandler<UserCreatedEvent>
{
  public async ValueTask Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling User Created event for user {userId} - {email}", 
      domainEvent.UserId, 
      domainEvent.Email);

    await emailSender.SendEmailAsync(
      domainEvent.Email,
      "no-reply@esteticaveros.com",
      "Bienvenido a Estética Veros",
      $"Hola {domainEvent.FullName}, tu cuenta ha sido creada exitosamente.");
  }
}
