namespace EsteticaVeros.API.Web.Customers;

public class CreateCustomerResponse(CustomerRecord customer)
{
  public CustomerRecord Customer { get; set; } = customer;
}