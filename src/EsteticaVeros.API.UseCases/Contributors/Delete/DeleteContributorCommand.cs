using EsteticaVeros.API.Core.ContributorAggregate;

namespace EsteticaVeros.API.UseCases.Contributors.Delete;

public record DeleteContributorCommand(ContributorId ContributorId) : ICommand<Result>;
