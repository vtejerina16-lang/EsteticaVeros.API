namespace EsteticaVeros.API.Core.CashRegisterAggregate;

public class MovementType : SmartEnum<MovementType>
{
  public static readonly MovementType Income = new(nameof(Income), 1, "Ingreso");
  public static readonly MovementType Expense = new(nameof(Expense), 2, "Egreso");
  public static readonly MovementType Withdrawal = new(nameof(Withdrawal), 3, "Retiro");
  public static readonly MovementType Deposit = new(nameof(Deposit), 4, "Depósito");
  public static readonly MovementType Transfer = new(nameof(Transfer), 5, "Transferencia");
  public static readonly MovementType Adjustment = new(nameof(Adjustment), 6, "Ajuste");

  private MovementType(string name, int value, string displayName) 
    : base(name, value)
  {
    DisplayName = displayName;
  }

  public string DisplayName { get; }

  public bool IsIncome => this == Income || this == Deposit;
  public bool IsExpense => this == Expense || this == Withdrawal;
}