using Vogen;

namespace EsteticaVeros.API.Core.CustomerAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct CustomerNumber
{
  private static Validation Validate(in string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Validation.Invalid("Customer number cannot be empty");

    if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^CL-\d{4}-\d{4}$"))
      return Validation.Invalid("Customer number must be in format: CL-YYYY-NNNN (e.g., CL-2024-0001)");

    return Validation.Ok;
  }

  public static CustomerNumber Generate(int year, int sequence)
  {
    var number = $"CL-{year:D4}-{sequence:D4}";
    return From(number);
  }
}