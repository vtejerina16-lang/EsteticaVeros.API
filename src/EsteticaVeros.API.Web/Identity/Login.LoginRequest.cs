using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Identity;

public class LoginRequest
{
  public const string Route = "/api/auth/login";

  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;

  [Required]
  public string Password { get; set; } = string.Empty;

  public bool RememberMe { get; set; } = false;
}