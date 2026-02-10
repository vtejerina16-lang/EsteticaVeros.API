namespace EsteticaVeros.API.Web.Identity;

public class UpdateUserResponse(UserRecord user)
{
  public UserRecord User { get; set; } = user;
}