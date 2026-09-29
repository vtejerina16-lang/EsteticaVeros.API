namespace EsteticaVeros.API.Web.Customers;

public record CustomersListResponse(
  IReadOnlyList<CustomerRecord> Customers,
  int Page,
  int PerPage,
  int TotalCount,
  int TotalPages
);