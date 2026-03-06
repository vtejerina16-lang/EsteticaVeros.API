using FluentValidation;

namespace EsteticaVeros.API.Web.Appointments;

public class CreateAppointmentValidator : Validator<CreateAppointmentRequest>
{
  public CreateAppointmentValidator()
  {
    RuleFor(x => x.CustomerId)
      .GreaterThan(0);

    RuleFor(x => x.AppointmentDate)
      .GreaterThan(DateTime.UtcNow)
      .WithMessage("Appointment date must be in the future");

    RuleFor(x => x.Source)
      .InclusiveBetween(1, 7)
      .WithMessage("Source must be between 1 and 7");

    RuleFor(x => x.ServiceIds)
      .NotEmpty()
      .WithMessage("At least one service is required");

    RuleFor(x => x.ServiceIds)
      .Must(ids => ids.All(id => id > 0))
      .When(x => x.ServiceIds != null && x.ServiceIds.Any())
      .WithMessage("All service IDs must be greater than 0");

    RuleFor(x => x.RecurrencePattern)
      .NotEmpty()
      .When(x => x.IsRecurring)
      .WithMessage("Recurrence pattern is required when IsRecurring is true");

    RuleFor(x => x.RecurrencePattern)
      .Must(pattern => pattern == "Weekly" || pattern == "Monthly")
      .When(x => x.IsRecurring && !string.IsNullOrWhiteSpace(x.RecurrencePattern))
      .WithMessage("Recurrence pattern must be 'Weekly' or 'Monthly'");
  }
}