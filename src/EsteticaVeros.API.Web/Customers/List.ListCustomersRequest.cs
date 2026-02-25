namespace EsteticaVeros.API.Web.Customers;

public class ListCustomersRequest
{
  [BindFrom("page")]
  public int Page { get; init; } = 1;

  [BindFrom("per_page")]
  public int PerPage { get; init; } = 10;

  [BindFrom("is_active")]
  public bool? IsActive { get; init; }

  [BindFrom("search")]
  public string? SearchTerm { get; init; }
}