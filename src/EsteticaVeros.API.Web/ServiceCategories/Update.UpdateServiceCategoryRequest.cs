using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class UpdateServiceCategoryRequest
{
  public const string Route = "/api/service-categories/{CategoryId:int}";
  public static string BuildRoute(int categoryId) => Route.Replace("{CategoryId:int}", categoryId.ToString());

  public int CategoryId { get; set; }

  [Required]
  public string Name { get; set; } = string.Empty;

  public string? Description { get; set; }

  public int DisplayOrder { get; set; } = 0;

  public string? IconName { get; set; }

  public string? ColorCode { get; set; }

  public bool IsActive { get; set; } = true;
}