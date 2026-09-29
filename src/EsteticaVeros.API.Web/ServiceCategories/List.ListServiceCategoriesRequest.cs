namespace EsteticaVeros.API.Web.ServiceCategories;

public class ListServiceCategoriesRequest
{
  [BindFrom("is_active")]
  public bool? IsActive { get; init; }
}