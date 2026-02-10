namespace EsteticaVeros.API.Core.IdentityAggregate.Events;

public sealed class UserLoggedInEvent : DomainEventBase
{
  public int UserId { get; set; }
  public string Email { get; set; } = string.Empty;
  public DateTime LoginTime { get; set; }
  
  public UserLoggedInEvent(int userId, string email, DateTime loginTime)
  {
    UserId = userId;
    Email = email;
    LoginTime = loginTime;
  }
}