namespace EsteticaVeros.API.Web.Customers;

public record CustomerRecord(
  int Id,
  string CustomerNumber,
  string FullName,
  string PhoneNumber,
  string? Email,
  int LoyaltyPoints,
  bool IsActive
);