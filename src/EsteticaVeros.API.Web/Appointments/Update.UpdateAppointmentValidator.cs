using FluentValidation;

namespace EsteticaVeros.API.Web.Appointments;

public class UpdateAppointmentValidator : Validator<UpdateAppointmentRequest>
{
  public UpdateAppointmentValidator()
  {
    RuleFor(x => x.AppointmentId)
      .GreaterThan(0);

    RuleFor(x => x.AppointmentDate)
      .GreaterThan(DateTime.UtcNow)
      .When(x => x.AppointmentDate.HasValue)
      .WithMessage("Appointment date must be in the future");

    RuleFor(x => x.ServiceIds)
      .Must(ids => ids!.All(id => id > 0))
      .When(x => x.ServiceIds != null && x.ServiceIds.Any())
      .WithMessage("All service IDs must be greater than 0");

    RuleFor(x => x.AssignedAssistantId)
      .GreaterThan(0)
      .When(x => x.AssignedAssistantId.HasValue)
      .WithMessage("Assistant ID must be greater than 0");
  }
}