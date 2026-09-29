namespace EsteticaVeros.API.Web.ServiceCategories;

public class CreateServiceCategoryResponse(ServiceCategoryRecord category)
{
  public ServiceCategoryRecord Category { get; set; } = category;
}