namespace EsteticaVeros.API.UseCases.Customers.Create;

public record CreateCustomerCommand(
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
  int? CreatedByUserId = null
) : ICommand<Result<CustomerDTO>>;