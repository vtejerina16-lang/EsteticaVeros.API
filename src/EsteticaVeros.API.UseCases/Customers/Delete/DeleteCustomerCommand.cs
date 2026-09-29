namespace EsteticaVeros.API.UseCases.Customers.Delete;

public record DeleteCustomerCommand(int CustomerId) : ICommand<Result>;