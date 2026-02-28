namespace EsteticaVeros.API.Core.ServiceAggregate.Specifications;

public sealed class FeaturedServicesSpec : Specification<Service>
{
  public FeaturedServicesSpec()
  {
    Query
      .Where(s => s.IsActive && s.IsFeatured)
      .OrderBy(s => s.DisplayOrder)
      .ThenBy(s => s.Name);
  }
}