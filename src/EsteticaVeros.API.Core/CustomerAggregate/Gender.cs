namespace EsteticaVeros.API.Core.CustomerAggregate;

public class Gender : SmartEnum<Gender>
{
  public static readonly Gender Male = new(nameof(Male), 1);
  public static readonly Gender Female = new(nameof(Female), 2);
  public static readonly Gender Other = new(nameof(Other), 3);
  public static readonly Gender PreferNotToSay = new(nameof(PreferNotToSay), 4);

  protected Gender(string name, int value) : base(name, value) { }
}