using Ardalis.Specification;

namespace EsteticaVeros.API.Core.IdentityAggregate.Specifications;

public sealed class UserByIdSpec : Specification<ApplicationUser>, ISingleResultSpecification<ApplicationUser>
{
  public UserByIdSpec(int userId)
  {
    Query.Where(u => u.Id == userId);
  }
}