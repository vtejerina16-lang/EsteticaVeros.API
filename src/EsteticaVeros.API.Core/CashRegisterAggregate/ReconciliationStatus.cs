namespace EsteticaVeros.API.Core.CashRegisterAggregate;

public class ReconciliationStatus : SmartEnum<ReconciliationStatus>
{
  public static readonly ReconciliationStatus Pending = new(nameof(Pending), 1, "Pendiente");
  public static readonly ReconciliationStatus Approved = new(nameof(Approved), 2, "Aprobado");
  public static readonly ReconciliationStatus Rejected = new(nameof(Rejected), 3, "Rechazado");

  private ReconciliationStatus(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }
}