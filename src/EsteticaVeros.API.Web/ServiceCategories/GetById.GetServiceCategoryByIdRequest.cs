namespace EsteticaVeros.API.Web.ServiceCategories;

public record GetServiceCategoryByIdRequest
{
  public const string Route = "/api/service-categories/{CategoryId:int}";
  public static string BuildRoute(int categoryId) => Route.Replace("{CategoryId:int}", categoryId.ToString());

  public int CategoryId { get; set; }
}