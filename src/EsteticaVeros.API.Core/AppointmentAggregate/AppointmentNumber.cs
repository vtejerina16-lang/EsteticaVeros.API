using Vogen;

namespace EsteticaVeros.API.Core.AppointmentAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct AppointmentNumber
{
  private static Validation Validate(in string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Validation.Invalid("Appointment number cannot be empty");

    if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^APT-\d{4}-\d{4}$"))
      return Validation.Invalid("Appointment number must be in format: APT-YYYY-NNNN (e.g., APT-2024-0001)");

    return Validation.Ok;
  }

  public static AppointmentNumber Generate(int year, int sequence)
  {
    var number = $"APT-{year:D4}-{sequence:D4}";
    return From(number);
  }
}