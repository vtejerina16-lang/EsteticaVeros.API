using FluentValidation;

namespace EsteticaVeros.API.Web.Appointments;

public class GetAvailableSlotsValidator : Validator<GetAvailableSlotsRequest>
{
  public GetAvailableSlotsValidator()
  {
    RuleFor(x => x.Date)
      .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
      .WithMessage("Date must be today or in the future");

    RuleFor(x => x.EstimatedDuration)
      .GreaterThan(0)
      .WithMessage("Estimated duration must be greater than 0");

    RuleFor(x => x.AssignedAssistantId)
      .GreaterThan(0)
      .When(x => x.AssignedAssistantId.HasValue)
      .WithMessage("Assistant ID must be greater than 0");
  }
}