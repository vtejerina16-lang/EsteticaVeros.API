using EsteticaVeros.API.Core.ContributorAggregate;

namespace EsteticaVeros.API.UseCases.Contributors.Update;

public record UpdateContributorCommand(ContributorId ContributorId, ContributorName NewName) : ICommand<Result<ContributorDto>>;
