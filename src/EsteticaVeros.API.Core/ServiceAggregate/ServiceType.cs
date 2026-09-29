namespace EsteticaVeros.API.Core.ServiceAggregate;

public class ServiceType : SmartEnum<ServiceType>
{
  public static readonly ServiceType Simple = new(nameof(Simple), 1, "Servicio Simple");
  public static readonly ServiceType Composite = new(nameof(Composite), 2, "Servicio Compuesto");
  public static readonly ServiceType Package = new(nameof(Package), 3, "Paquete");

  private ServiceType(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }
}