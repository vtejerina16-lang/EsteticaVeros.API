using Vogen;

namespace EsteticaVeros.API.Core.ServiceAggregate;

[ValueObject<string>(conversions: Conversions.SystemTextJson)]
public partial struct ServiceNumber
{
  private static Validation Validate(in string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Validation.Invalid("Service number cannot be empty");

    // Acepta SRV-NNNN o PKG-NNNN
    if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^(SRV|PKG)-\d{4}$"))
      return Validation.Invalid("Service number must be in format: SRV-NNNN or PKG-NNNN (e.g., SRV-0001, PKG-0001)");

    return Validation.Ok;
  }

  public static ServiceNumber Generate(int sequence, bool isPackage = false)
  {
    var prefix = isPackage ? "PKG" : "SRV";
    var number = $"{prefix}-{sequence:D4}";
    return From(number);
  }
}