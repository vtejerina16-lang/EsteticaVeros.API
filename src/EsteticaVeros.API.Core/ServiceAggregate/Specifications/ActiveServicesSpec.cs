namespace EsteticaVeros.API.Core.ServiceAggregate.Specifications;

public sealed class ActiveServicesSpec : Specification<Service>
{
  public ActiveServicesSpec()
  {
    Query
      .Where(s => s.IsActive)
      .OrderBy(s => s.DisplayOrder)
      .ThenBy(s => s.Name);
  }
}