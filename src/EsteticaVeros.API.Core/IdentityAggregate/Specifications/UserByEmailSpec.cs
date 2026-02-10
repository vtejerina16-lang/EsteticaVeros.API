using Ardalis.Specification;

namespace EsteticaVeros.API.Core.IdentityAggregate.Specifications;

public sealed class UserByEmailSpec : Specification<ApplicationUser>, ISingleResultSpecification<ApplicationUser>
{
  public UserByEmailSpec(string email)
  {
    Query.Where(u => u.Email == email);
  }
}