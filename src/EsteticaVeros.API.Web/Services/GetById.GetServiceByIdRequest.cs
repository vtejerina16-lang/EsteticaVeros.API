namespace EsteticaVeros.API.Web.Services;

public record GetServiceByIdRequest
{
  public const string Route = "/api/services/{ServiceId:int}";
  public static string BuildRoute(int serviceId) => Route.Replace("{ServiceId:int}", serviceId.ToString());

  public int ServiceId { get; set; }
}