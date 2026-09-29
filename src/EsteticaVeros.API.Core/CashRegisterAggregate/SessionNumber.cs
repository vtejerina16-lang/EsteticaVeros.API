using Vogen;

namespace EsteticaVeros.API.Core.CashRegisterAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct SessionNumber
{
  private static Validation Validate(in string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Validation.Invalid("Session number cannot be empty");

    if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^SES-\d{4}-\d{4}$"))
      return Validation.Invalid("Session number must be in format: SES-YYYY-NNNN (e.g., SES-2026-0001)");

    return Validation.Ok;
  }

  public static SessionNumber Generate(int year, int sequence)
  {
    var number = $"SES-{year:D4}-{sequence:D4}";
    return From(number);
  }
}