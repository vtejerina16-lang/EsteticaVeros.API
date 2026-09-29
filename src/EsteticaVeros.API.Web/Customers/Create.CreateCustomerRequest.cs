using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Customers;

public class CreateCustomerRequest
{
  public const string Route = "/api/customers";

  [Required]
  public string FirstName { get; set; } = string.Empty;

  [Required]
  public string LastName { get; set; } = string.Empty;

  [Required]
  public string PhoneNumber { get; set; } = string.Empty;

  [Required]
  public int Gender { get; set; }

  [EmailAddress]
  public string? Email { get; set; }

  public DateTime? DateOfBirth { get; set; }

  public string? Address { get; set; }

  public string? City { get; set; }

  public string? Allergies { get; set; }

  public string? MedicalConditions { get; set; }

  public string? Medications { get; set; }

  public int? PreferredAssistantId { get; set; }
}