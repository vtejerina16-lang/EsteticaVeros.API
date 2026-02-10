using EsteticaVeros.API.Core.IdentityAggregate;

namespace EsteticaVeros.API.Core.Interfaces;

public interface IJwtTokenGenerator
{
  string GenerateToken(ApplicationUser user, string role);
}