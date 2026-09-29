using Microsoft.AspNetCore.Identity;

namespace EsteticaVeros.API.Core.IdentityAggregate;

public class ApplicationUser : IdentityUser<int>
{
  // Campos adicionales personalizados para EsteticaVeros
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string? Address { get; set; }
  public DateTime? DateOfBirth { get; set; }
  public string? ProfilePictureUrl { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? LastLoginAt { get; set; }
  
  // Propiedades calculadas
  public string FullName => $"{FirstName} {LastName}";
  
  // Métodos de negocio
  public void UpdateProfile(string firstName, string lastName, string? address, DateTime? dateOfBirth)
  {
    FirstName = firstName;
    LastName = lastName;
    Address = address;
    DateOfBirth = dateOfBirth;
  }
  
  public void RecordLogin()
  {
    LastLoginAt = DateTime.UtcNow;
  }
  
  public void Activate() => IsActive = true;
  public void Deactivate() => IsActive = false;
}