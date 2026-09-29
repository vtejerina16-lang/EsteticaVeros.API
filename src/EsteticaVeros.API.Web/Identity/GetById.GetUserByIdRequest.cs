namespace EsteticaVeros.API.Web.Identity;

public class GetUserByIdRequest
{
  public const string Route = "/api/users/{UserId:int}";
  public static string BuildRoute(int userId) => Route.Replace("{UserId:int}", userId.ToString());

  public int UserId { get; set; }
}
