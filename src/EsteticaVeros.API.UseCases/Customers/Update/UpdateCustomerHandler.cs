using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Customers.Update;

public class UpdateCustomerHandler(
  IRepository<Customer> repository,
  ILogger<UpdateCustomerHandler> logger) : ICommandHandler<UpdateCustomerCommand, Result<CustomerDTO>>
{
  public async ValueTask<Result<CustomerDTO>> Handle(
    UpdateCustomerCommand command,
    CancellationToken cancellationToken)
  {
    var spec = new CustomerByIdSpec(command.CustomerId);
    var customer = await repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (customer == null)
    {
      return Result.NotFound();
    }

    // Check if phone number is being changed and if it's already in use
    if (customer.PhoneNumber != command.PhoneNumber)
    {
      var existingCustomer = await repository.FirstOrDefaultAsync(
        new CustomerByPhoneNumberSpec(command.PhoneNumber),
        cancellationToken);

      if (existingCustomer != null && existingCustomer.Id != customer.Id)
      {
        return Result.Error("Another customer with this phone number already exists.");
      }
    }

    // Check if email is being changed and if it's already in use
    if (!string.IsNullOrWhiteSpace(command.Email) && customer.Email != command.Email)
    {
      var existingByEmail = await repository.FirstOrDefaultAsync(
        new CustomerByEmailSpec(command.Email),
        cancellationToken);

      if (existingByEmail != null && existingByEmail.Id != customer.Id)
      {
        return Result.Error("Another customer with this email already exists.");
      }
    }

    // Update basic info
    var gender = Gender.FromValue(command.Gender);
    customer.UpdateBasicInfo(
      command.FirstName,
      command.LastName,
      command.PhoneNumber,
      gender
    );

    // Update optional fields
    customer.UpdateEmail(command.Email);
    customer.UpdateDateOfBirth(command.DateOfBirth);
    customer.UpdateContactInfo(command.Address, command.City);
    customer.UpdateMedicalInfo(command.Allergies, command.MedicalConditions, command.Medications);
    customer.UpdatePreferredAssistant(command.PreferredAssistantId);

    // Update active status
    if (command.IsActive && !customer.IsActive)
    {
      customer.Activate();
    }
    else if (!command.IsActive && customer.IsActive)
    {
      customer.Deactivate();
    }

    await repository.UpdateAsync(customer, cancellationToken);

    logger.LogInformation(
      "Customer updated: {CustomerNumber} - {FullName}",
      customer.CustomerNumber,
      customer.FullName);

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