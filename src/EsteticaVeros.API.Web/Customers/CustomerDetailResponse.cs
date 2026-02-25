namespace EsteticaVeros.API.Web.Customers;

public record CustomerDetailResponse(
  int Id,
  string CustomerNumber,
  string FirstName,
  string LastName,
  string FullName,
  string PhoneNumber,
  string? Email,
  DateTime? DateOfBirth,
  int? Age,
  string Gender,
  string? Address,
  string? City,
  string? Allergies,
  string? MedicalConditions,
  string? Medications,
  int? PreferredAssistantId,
  string? ProfilePictureUrl,
  int LoyaltyPoints,
  decimal TotalSpent,
  int TotalVisits,
  DateTime? LastVisitDate,
  bool IsActive,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);