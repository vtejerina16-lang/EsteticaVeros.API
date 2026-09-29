using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Customers.Get;

public class GetCustomerHandler(IRepository<Customer> repository)
  : IQueryHandler<GetCustomerQuery, Result<CustomerDTO>>
{
  public async ValueTask<Result<CustomerDTO>> Handle(
    GetCustomerQuery query,
    CancellationToken cancellationToken)
  {
    var spec = new CustomerByIdSpec(query.CustomerId);
    var customer = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (customer == null)
    {
      return Result.NotFound();
    }

    return Result.Success(MapToDTO(customer));
  }

  private static CustomerDTO MapToDTO(Customer customer)
  {
    return new CustomerDTO(
      customer.Id,
      customer.CustomerNumber.Value,
      customer.FirstName,
      customer.LastName,
      customer.FullName,
      customer.PhoneNumber,
      customer.Email,
      customer.DateOfBirth,
      customer.Age,
      customer.Gender.Name,
      customer.Address,
      customer.City,
      customer.Allergies,
      customer.MedicalConditions,
      customer.Medications,
      customer.PreferredAssistantId,
      customer.ProfilePictureUrl,
      customer.LoyaltyPoints,
      customer.TotalSpent,
      customer.TotalVisits,
      customer.LastVisitDate,
      customer.IsActive,
      customer.CreatedAt,
      customer.UpdatedAt
    );
  }
}