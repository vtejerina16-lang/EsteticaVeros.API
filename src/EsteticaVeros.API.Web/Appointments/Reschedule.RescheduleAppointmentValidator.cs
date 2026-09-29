using FluentValidation;

namespace EsteticaVeros.API.Web.Appointments;

public class RescheduleAppointmentValidator : Validator<RescheduleAppointmentRequest>
{
  public RescheduleAppointmentValidator()
  {
    RuleFor(x => x.AppointmentId)
      .GreaterThan(0);

    RuleFor(x => x.NewAppointmentDate)
      .GreaterThan(DateTime.UtcNow)
      .WithMessage("New appointment date must be in the future");

    RuleFor(x => x.Reason)
      .NotEmpty()
      .MinimumLength(5)
      .MaximumLength(500);
  }
}