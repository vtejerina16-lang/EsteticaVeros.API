using Ardalis.Specification;

namespace EsteticaVeros.API.Core.IdentityAggregate.Specifications;

public sealed class ActiveUsersSpec : Specification<ApplicationUser>
{
  public ActiveUsersSpec()
  {
    Query
      .Where(u => u.IsActive)
      .OrderBy(u => u.FirstName)
      .ThenBy(u => u.LastName);
  }
}