namespace EsteticaVeros.API.Web.CashMovements;

public class GetCashMovementByIdRequest
{
  public const string Route = "/api/cash-movements/{MovementId:int}";
  public static string BuildRoute(int movementId) 
    => Route.Replace("{MovementId:int}", movementId.ToString());

  public int MovementId { get; init; }
}