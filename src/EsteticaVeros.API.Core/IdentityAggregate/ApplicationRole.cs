using Microsoft.AspNetCore.Identity;

namespace EsteticaVeros.API.Core.IdentityAggregate;

public class ApplicationRole : IdentityRole<int>
{
  // Campos adicionales personalizados
  public string? Description { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  
  public ApplicationRole() : base()
  {
  }
  
  public ApplicationRole(string roleName, string? description = null) : base(roleName)
  {
    Description = description;
  }
  
  public void UpdateDescription(string description)
  {
    Description = description;
  }
  
  public void Activate() => IsActive = true;
  public void Deactivate() => IsActive = false;
}