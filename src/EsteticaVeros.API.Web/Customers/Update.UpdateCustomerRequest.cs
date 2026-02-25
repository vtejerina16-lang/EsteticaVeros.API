using System.ComponentModel.DataAnnotations;

namespace EsteticaVeros.API.Web.Customers;

public class UpdateCustomerRequest
{
  public const string Route = "/api/customers/{CustomerId:int}";
  public static string BuildRoute(int customerId) => Route.Replace("{CustomerId:int}", customerId.ToString());

  public int CustomerId { get; set; }

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

  public bool IsActive { get; set; } = true;
}