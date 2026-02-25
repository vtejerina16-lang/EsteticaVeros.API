namespace EsteticaVeros.API.Web.Customers;

public class UpdateCustomerResponse(CustomerDetailResponse customer)
{
  public CustomerDetailResponse Customer { get; set; } = customer;
}