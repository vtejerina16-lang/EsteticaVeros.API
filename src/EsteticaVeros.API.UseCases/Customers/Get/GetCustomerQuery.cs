namespace EsteticaVeros.API.UseCases.Customers.Get;

public record GetCustomerQuery(int CustomerId) : IQuery<Result<CustomerDTO>>;