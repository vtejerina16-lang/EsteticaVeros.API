namespace EsteticaVeros.API.Core.AppointmentAggregate;

public class AppointmentStatus : SmartEnum<AppointmentStatus>
{
  public static readonly AppointmentStatus Scheduled = new(nameof(Scheduled), 1, "Programada");
  public static readonly AppointmentStatus Confirmed = new(nameof(Confirmed), 2, "Confirmada");
  public static readonly AppointmentStatus CheckedIn = new(nameof(CheckedIn), 3, "Cliente llegó");
  public static readonly AppointmentStatus InProgress = new(nameof(InProgress), 4, "En progreso");
  public static readonly AppointmentStatus Completed = new(nameof(Completed), 5, "Completada");
  public static readonly AppointmentStatus Cancelled = new(nameof(Cancelled), 6, "Cancelada");
  public static readonly AppointmentStatus NoShow = new(nameof(NoShow), 7, "No se presentó");
  public static readonly AppointmentStatus Rescheduled = new(nameof(Rescheduled), 8, "Reprogramada");

  private AppointmentStatus(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }
}