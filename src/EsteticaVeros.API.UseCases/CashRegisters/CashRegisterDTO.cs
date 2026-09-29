namespace EsteticaVeros.API.UseCases.CashRegisters;

public record CashRegisterDTO(
  int Id,
  string Name,
  string? Location,
  bool IsActive,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);