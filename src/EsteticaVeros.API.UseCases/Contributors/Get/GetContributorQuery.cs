using EsteticaVeros.API.Core.ContributorAggregate;

namespace EsteticaVeros.API.UseCases.Contributors.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
