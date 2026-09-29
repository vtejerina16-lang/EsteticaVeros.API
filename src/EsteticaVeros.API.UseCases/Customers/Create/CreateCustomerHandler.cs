using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Customers.Create;

public class CreateCustomerHandler(
  IRepository<Customer> repository,
  ILogger<CreateCustomerHandler> logger) : ICommandHandler<CreateCustomerCommand, Result<CustomerDTO>>
{
  public async ValueTask<Result<CustomerDTO>> Handle(
    CreateCustomerCommand command,
    CancellationToken cancellationToken)
  {
    // Check if phone number already exists
    var existingCustomer = await repository.FirstOrDefaultAsync(
      new CustomerByPhoneNumberSpec(command.PhoneNumber),
      cancellationToken);

    if (existingCustomer != null)
    {
      return Result.Error("A customer with this phone number already exists.");
    }

    // Check if email exists (if provided)
    if (!string.IsNullOrWhiteSpace(command.Email))
    {
      var existingByEmail = await repository.FirstOrDefaultAsync(
        new CustomerByEmailSpec(command.Email),
        cancellationToken);

      if (existingByEmail != null)
      {
        return Result.Error("A customer with this email already exists.");
      }
    }

    // Generate customer number
    var year = DateTime.UtcNow.Year;
    var sequence = await GetNextSequenceNumber(year, cancellationToken);
    var customerNumber = CustomerNumber.Generate(year, sequence);

    // Create customer
    var gender = Gender.FromValue(command.Gender);
    
    var customer = new Customer(
      customerNumber,
      command.FirstName,
      command.LastName,
      command.PhoneNumber,
      gender
    );

    // Update optional fields
    if (!string.IsNullOrWhiteSpace(command.Email))
    {
      customer.UpdateEmail(command.Email);
    }

    if (command.DateOfBirth.HasValue)
    {
      customer.UpdateDateOfBirth(command.DateOfBirth);
    }

    if (!string.IsNullOrWhiteSpace(command.Address) || !string.IsNullOrWhiteSpace(command.City))
    {
      customer.UpdateContactInfo(command.Address, command.City);
    }

    if (!string.IsNullOrWhiteSpace(command.Allergies) ||
        !string.IsNullOrWhiteSpace(command.MedicalConditions) ||
        !string.IsNullOrWhiteSpace(command.Medications))
    {
      customer.UpdateMedicalInfo(command.Allergies, command.MedicalConditions, command.Medications);
    }

    if (command.PreferredAssistantId.HasValue)
    {
      customer.UpdatePreferredAssistant(command.PreferredAssistantId);
    }

    if (command.CreatedByUserId.HasValue)
    {
      customer.UpdateCreatedBy(command.CreatedByUserId.Value);
    }

    var createdCustomer = await repository.AddAsync(customer, cancellationToken);

    logger.LogInformation(
      "Customer created: {CustomerNumber} - {FullName} ({PhoneNumber})",
      createdCustomer.CustomerNumber,
      createdCustomer.FullName,
      createdCustomer.PhoneNumber);

    return Result.Success(MapToDTO(createdCustomer));
  }

  private async Task<int> GetNextSequenceNumber(int year, CancellationToken cancellationToken)
  {
    var customers = await repository.ListAsync(cancellationToken);
    
    var yearPrefix = $"CL-{year:D4}-";
    var highestSequence = customers
      .Where(c => c.CustomerNumber.Value.StartsWith(yearPrefix))
      .Select(c =>
      {
        var parts = c.CustomerNumber.Value.Split('-');
        return parts.Length == 3 && int.TryParse(parts[2], out var seq) ? seq : 0;
      })
      .DefaultIfEmpty(0)
      .Max();

    return highestSequence + 1;
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