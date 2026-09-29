using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Identity;

public class CreateUserRequest
{
  public const string Route = "/api/users";

  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;

  [Required]
  public string Password { get; set; } = string.Empty;

  [Required]
  public string FirstName { get; set; } = string.Empty;

  [Required]
  public string LastName { get; set; } = string.Empty;

  public string? PhoneNumber { get; set; }

  [Required]
  public string RoleName { get; set; } = "Receptionist";
}