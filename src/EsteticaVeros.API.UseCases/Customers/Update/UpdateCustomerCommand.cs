namespace EsteticaVeros.API.UseCases.Customers.Update;

public record UpdateCustomerCommand(
  int CustomerId,
  string FirstName,
  string LastName,
  string PhoneNumber,
  int Gender,
  string? Email = null,
  DateTime? DateOfBirth = null,
  string? Address = null,
  string? City = null,
  string? Allergies = null,
  string? MedicalConditions = null,
  string? Medications = null,
  int? PreferredAssistantId = null,
  bool IsActive = true
) : ICommand<Result<CustomerDTO>>;