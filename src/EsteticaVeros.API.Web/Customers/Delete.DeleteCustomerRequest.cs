namespace EsteticaVeros.API.Web.Customers;

public record DeleteCustomerRequest
{
  public const string Route = "/api/customers/{CustomerId:int}";
  public static string BuildRoute(int customerId) => Route.Replace("{CustomerId:int}", customerId.ToString());

  public int CustomerId { get; set; }
}