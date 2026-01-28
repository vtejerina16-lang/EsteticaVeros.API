using EsteticaVeros.API.Core.ContributorAggregate;
using Vogen;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
internal partial class VogenEfCoreConverters;
