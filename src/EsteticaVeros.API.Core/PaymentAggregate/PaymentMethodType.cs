namespace EsteticaVeros.API.Core.PaymentAggregate;

public class PaymentMethodType : SmartEnum<PaymentMethodType>
{
  public static readonly PaymentMethodType Cash = new(nameof(Cash), 1, "Efectivo");
  public static readonly PaymentMethodType Card = new(nameof(Card), 2, "Tarjeta");
  public static readonly PaymentMethodType Transfer = new(nameof(Transfer), 3, "Transferencia");
  public static readonly PaymentMethodType QR = new(nameof(QR), 4, "QR");
  public static readonly PaymentMethodType Check = new(nameof(Check), 5, "Cheque");
  public static readonly PaymentMethodType Other = new(nameof(Other), 6, "Otro");

  private PaymentMethodType(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }
}