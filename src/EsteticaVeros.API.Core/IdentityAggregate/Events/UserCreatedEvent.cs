namespace EsteticaVeros.API.Core.IdentityAggregate.Events;

/// <summary>
/// A domain event that is dispatched whenever a user is created.
/// The CreateUserService is used to dispatch this event.
/// NOTE: Would prefer this be internal but Mediator Source Generator needs access to it from elsewhere.
/// </summary>
public sealed class UserCreatedEvent : DomainEventBase
{
  public int UserId { get; set; }
  public string Email { get; set; } = string.Empty;
  public string FullName { get; set; } = string.Empty;
  
  public UserCreatedEvent(int userId, string email, string fullName)
  {
    UserId = userId;
    Email = email;
    FullName = fullName;
  }
}