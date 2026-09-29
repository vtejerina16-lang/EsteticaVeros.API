namespace EsteticaVeros.API.Core.PaymentAggregate;

public class PaymentStatus : SmartEnum<PaymentStatus>
{
  public static readonly PaymentStatus Pending = new(nameof(Pending), 1, "Pendiente");
  public static readonly PaymentStatus Completed = new(nameof(Completed), 2, "Completado");
  public static readonly PaymentStatus Refunded = new(nameof(Refunded), 3, "Reembolsado");
  public static readonly PaymentStatus Cancelled = new(nameof(Cancelled), 4, "Cancelado");

  private PaymentStatus(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }
}