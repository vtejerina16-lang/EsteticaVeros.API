using Vogen;

namespace EsteticaVeros.API.Core.ServiceCategoryAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct CategoryNumber
{
  private static Validation Validate(in string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Validation.Invalid("Category number cannot be empty");

    if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^CAT-\d{4}$"))
      return Validation.Invalid("Category number must be in format: CAT-NNNN (e.g., CAT-0001)");

    return Validation.Ok;
  }

  public static CategoryNumber Generate(int sequence)
  {
    var number = $"CAT-{sequence:D4}";
    return From(number);
  }
}