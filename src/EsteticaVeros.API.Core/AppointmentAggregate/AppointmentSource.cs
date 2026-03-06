namespace EsteticaVeros.API.Core.AppointmentAggregate;

public class AppointmentSource : SmartEnum<AppointmentSource>
{
  public static readonly AppointmentSource InOffice = new(nameof(InOffice), 1, "En Oficina", "Registrado en recepción");
  public static readonly AppointmentSource WhatsApp = new(nameof(WhatsApp), 2, "WhatsApp", "Registrado vía WhatsApp");
  public static readonly AppointmentSource Phone = new(nameof(Phone), 3, "Teléfono", "Registrado por llamada telefónica");
  public static readonly AppointmentSource WebForm = new(nameof(WebForm), 4, "Formulario Web", "Registrado desde sitio web");
  public static readonly AppointmentSource Facebook = new(nameof(Facebook), 5, "Facebook", "Registrado vía Facebook Messenger");
  public static readonly AppointmentSource Instagram = new(nameof(Instagram), 6, "Instagram", "Registrado vía Instagram DM");
  public static readonly AppointmentSource Mobile = new(nameof(Mobile), 7, "App Móvil", "Registrado desde app móvil");

  private AppointmentSource(string name, int value, string displayName, string description) 
    : base(name, value)
  {
    DisplayName = displayName;
    Description = description;
  }

  public string DisplayName { get; }
  public string Description { get; }
}