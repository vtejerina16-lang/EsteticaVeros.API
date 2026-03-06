namespace EsteticaVeros.API.Core.AppointmentAggregate;

public class ServiceStatus : SmartEnum<ServiceStatus>
{
  public static readonly ServiceStatus Pending = new(nameof(Pending), 1, "Pendiente");
  public static readonly ServiceStatus InProgress = new(nameof(InProgress), 2, "En progreso");
  public static readonly ServiceStatus Completed = new(nameof(Completed), 3, "Completado");
  public static readonly ServiceStatus Skipped = new(nameof(Skipped), 4, "Omitido");

  private ServiceStatus(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }
}