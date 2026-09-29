using EsteticaVeros.API.Core.PaymentAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.Payments.Refund;

public class RefundPaymentHandler(
  IRepository<Payment> paymentRepository,
  IRepository<CashMovement> movementRepository,
  IRepository<CashRegisterSession> sessionRepository)
  : ICommandHandler<RefundPaymentCommand, Result>
{
  public async ValueTask<Result> Handle(
    RefundPaymentCommand command,
    CancellationToken cancellationToken)
  {
    var payment = await paymentRepository.GetByIdAsync(command.PaymentId, cancellationToken);

    if (payment == null)
    {
      return Result.NotFound("Payment not found");
    }

    try
    {
      payment.Refund(command.Reason);

      await paymentRepository.UpdateAsync(payment, cancellationToken);

      // Si está asociado a una sesión de caja, crear movimiento de egreso
      if (payment.CashRegisterSessionId.HasValue)
      {
        var session = await sessionRepository.GetByIdAsync(payment.CashRegisterSessionId.Value, cancellationToken);
        
        if (session != null && session.IsOpen)
        {
          var year = DateTime.UtcNow.Year;
          var allMovements = await movementRepository.ListAsync(cancellationToken);
          var movementsThisYear = allMovements.Count(m => m.CreatedAt.Year == year);
          var movementNumber = MovementNumber.Generate(year, movementsThisYear + 1);

          var cashMovement = new CashMovement(
            movementNumber,
            payment.CashRegisterSessionId.Value,
            MovementType.Expense,
            payment.Amount,
            $"Reembolso de pago: {payment.PaymentNumber.Value}",
            payment.Id,
            payment.Reference
          );

          await movementRepository.AddAsync(cashMovement, cancellationToken);
          
          // Actualizar expected amount de la sesión
          session.AddExpense(payment.Amount);
        }
      }

      await paymentRepository.SaveChangesAsync(cancellationToken);

      return Result.Success();
    }
    catch (InvalidOperationException ex)
    {
      return Result.Error(ex.Message);
    }
  }
}