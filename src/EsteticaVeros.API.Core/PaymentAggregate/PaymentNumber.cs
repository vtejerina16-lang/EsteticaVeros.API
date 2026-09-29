using Vogen;

namespace EsteticaVeros.API.Core.PaymentAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct PaymentNumber
{
  private static Validation Validate(in string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Validation.Invalid("Payment number cannot be empty");

    if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^PAY-\d{4}-\d{4}$"))
      return Validation.Invalid("Payment number must be in format: PAY-YYYY-NNNN (e.g., PAY-2026-0001)");

    return Validation.Ok;
  }

  public static PaymentNumber Generate(int year, int sequence)
  {
    var number = $"PAY-{year:D4}-{sequence:D4}";
    return From(number);
  }
}