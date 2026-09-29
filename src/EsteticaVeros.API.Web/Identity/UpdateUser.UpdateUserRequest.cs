using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Identity;

public class UpdateUserRequest
{
  public const string Route = "/api/users/{UserId:int}";
  public static string BuildRoute(int userId) => Route.Replace("{UserId:int}", userId.ToString());

  public int UserId { get; set; }

  [Required]
  public string FirstName { get; set; } = string.Empty;

  [Required]
  public string LastName { get; set; } = string.Empty;

  public string? PhoneNumber { get; set; }

  public string? Address { get; set; }

  public DateTime? DateOfBirth { get; set; }

  [Required]
  public string RoleName { get; set; } = string.Empty;

  public bool IsActive { get; set; } = true;
}