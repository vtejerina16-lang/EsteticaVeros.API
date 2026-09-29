namespace EsteticaVeros.API.Core.CashRegisterAggregate;

public class SessionStatus : SmartEnum<SessionStatus>
{
  public static readonly SessionStatus Open = new(nameof(Open), 1, "Abierta");
  public static readonly SessionStatus Closed = new(nameof(Closed), 2, "Cerrada");
  public static readonly SessionStatus Reconciled = new(nameof(Reconciled), 3, "Arqueo Completado");

  private SessionStatus(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }
}