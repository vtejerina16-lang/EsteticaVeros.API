using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class CreateServiceCategoryRequest
{
  public const string Route = "/api/service-categories";

  [Required]
  public string Name { get; set; } = string.Empty;

  public string? Description { get; set; }

  public int DisplayOrder { get; set; } = 0;

  public string? IconName { get; set; }

  public string? ColorCode { get; set; }
}