using EsteticaVeros.API.Core.PaymentAggregate;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CashRegisterAggregate;

namespace EsteticaVeros.API.UseCases.Payments.Create;

public class CreatePaymentHandler(
  IRepository<Payment> paymentRepository,
  IReadRepository<Customer> customerRepository,
  IReadRepository<PaymentMethod> paymentMethodRepository,
  IRepository<CashRegisterSession> sessionRepository,
  IRepository<CashMovement> movementRepository)
  : ICommandHandler<CreatePaymentCommand, Result<int>>
{
  public async ValueTask<Result<int>> Handle(
    CreatePaymentCommand command,
    CancellationToken cancellationToken)
  {
    // Validar que el cliente existe
    var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
    if (customer == null)
    {
      return Result.NotFound("Customer not found");
    }

    // Validar que el método de pago existe
    var paymentMethod = await paymentMethodRepository.GetByIdAsync(command.PaymentMethodId, cancellationToken);
    if (paymentMethod == null)
    {
      return Result.NotFound("Payment method not found");
    }

    // Si el método de pago requiere caja, validar sesión
    if (paymentMethod.RequiresCashRegister)
    {
      if (!command.CashRegisterSessionId.HasValue)
      {
        return Result.Error("This payment method requires a cash register session");
      }

      var session = await sessionRepository.GetByIdAsync(command.CashRegisterSessionId.Value, cancellationToken);
      if (session == null)
      {
        return Result.NotFound("Cash register session not found");
      }

      if (!session.IsOpen)
      {
        return Result.Error("Cash register session is not open");
      }
    }

    // Generar número de pago
    var year = DateTime.UtcNow.Year;
    var allPayments = await paymentRepository.ListAsync(cancellationToken);
    var paymentsThisYear = allPayments.Count(p => p.CreatedAt.Year == year);
    var paymentNumber = PaymentNumber.Generate(year, paymentsThisYear + 1);

    // Crear pago
    var payment = new Payment(
      paymentNumber,
      command.CustomerId,
      command.PaymentMethodId,
      command.Amount,
      command.AppointmentId,
      command.CashRegisterSessionId,
      command.Reference,
      command.Notes,
      command.CreatedByUserId
    );

    var createdPayment = await paymentRepository.AddAsync(payment, cancellationToken);
    await paymentRepository.SaveChangesAsync(cancellationToken);

    // Si requiere caja, crear movimiento automático
    if (paymentMethod.RequiresCashRegister && command.CashRegisterSessionId.HasValue)
    {
      var movementYear = DateTime.UtcNow.Year;
      var allMovements = await movementRepository.ListAsync(cancellationToken);
      var movementsThisYear = allMovements.Count(m => m.CreatedAt.Year == movementYear);
      var movementNumber = MovementNumber.Generate(movementYear, movementsThisYear + 1);

      var cashMovement = new CashMovement(
        movementNumber,
        command.CashRegisterSessionId.Value,
        MovementType.Income,
        command.Amount,
        $"Pago de cliente: {customer.FullName}",
        createdPayment.Id,
        command.Reference,
        command.CreatedByUserId
      );

      await movementRepository.AddAsync(cashMovement, cancellationToken);
      
      // Actualizar expected amount de la sesión
      var session = await sessionRepository.GetByIdAsync(command.CashRegisterSessionId.Value, cancellationToken);
      session?.AddIncome(command.Amount);
      
      await movementRepository.SaveChangesAsync(cancellationToken);
    }

    return Result.Success(createdPayment.Id);
  }
}