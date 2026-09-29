namespace EsteticaVeros.API.Web.ServiceCategories;

public class UpdateServiceCategoryResponse(ServiceCategoryDetailResponse category)
{
  public ServiceCategoryDetailResponse Category { get; set; } = category;
}