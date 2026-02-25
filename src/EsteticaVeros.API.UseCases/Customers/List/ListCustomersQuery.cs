namespace EsteticaVeros.API.UseCases.Customers.List;

public record ListCustomersQuery(
  int Page = 1,
  int PerPage = 10,
  bool? IsActive = null,
  string? SearchTerm = null
) : IQuery<Result<PagedResult<CustomerDTO>>>;