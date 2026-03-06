using FluentValidation;

namespace EsteticaVeros.API.Web.Appointments;

public class CancelAppointmentValidator : Validator<CancelAppointmentRequest>
{
  public CancelAppointmentValidator()
  {
    RuleFor(x => x.AppointmentId)
      .GreaterThan(0);

    RuleFor(x => x.CancellationReason)
      .NotEmpty()
      .MinimumLength(5)
      .MaximumLength(500);
  }
}