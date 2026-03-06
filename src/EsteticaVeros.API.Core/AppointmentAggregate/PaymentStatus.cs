namespace EsteticaVeros.API.Core.AppointmentAggregate;

public class PaymentStatus : SmartEnum<PaymentStatus>
{
  public static readonly PaymentStatus Pending = new(nameof(Pending), 1, "Pendiente");
  public static readonly PaymentStatus DepositPaid = new(nameof(DepositPaid), 2, "Depósito pagado");
  public static readonly PaymentStatus Paid = new(nameof(Paid), 3, "Pagado");
  public static readonly PaymentStatus PartiallyPaid = new(nameof(PartiallyPaid), 4, "Parcialmente pagado");
  public static readonly PaymentStatus Refunded = new(nameof(Refunded), 5, "Reembolsado");

  private PaymentStatus(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }
}