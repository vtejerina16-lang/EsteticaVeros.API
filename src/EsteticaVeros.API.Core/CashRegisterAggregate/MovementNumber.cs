using Vogen;

namespace EsteticaVeros.API.Core.CashRegisterAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct MovementNumber
{
  private static Validation Validate(in string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Validation.Invalid("Movement number cannot be empty");

    if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^MOV-\d{4}-\d{4}$"))
      return Validation.Invalid("Movement number must be in format: MOV-YYYY-NNNN (e.g., MOV-2026-0001)");

    return Validation.Ok;
  }

  public static MovementNumber Generate(int year, int sequence)
  {
    var number = $"MOV-{year:D4}-{sequence:D4}";
    return From(number);
  }
}