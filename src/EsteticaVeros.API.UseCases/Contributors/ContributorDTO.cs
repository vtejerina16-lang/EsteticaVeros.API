using EsteticaVeros.API.Core.ContributorAggregate;

namespace EsteticaVeros.API.UseCases.Contributors;
public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
