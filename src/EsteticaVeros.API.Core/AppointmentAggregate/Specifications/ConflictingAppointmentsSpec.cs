namespace EsteticaVeros.API.Core.AppointmentAggregate.Specifications;

public sealed class ConflictingAppointmentsSpec : Specification<Appointment>
{
  public ConflictingAppointmentsSpec(
    DateTime startTime,
    int durationMinutes,
    int? assistantId = null,
    int? excludeAppointmentId = null)
  {
    var endTime = startTime.AddMinutes(durationMinutes);

    Query.Where(a =>
      a.Status != AppointmentStatus.Cancelled &&
      a.Status != AppointmentStatus.NoShow &&
      a.Status != AppointmentStatus.Rescheduled &&
      a.AppointmentDate < endTime);

    // Calculate estimated end time for existing appointments
    Query.Where(a => a.AppointmentDate.AddMinutes(a.EstimatedDuration) > startTime);

    if (assistantId.HasValue)
    {
      Query.Where(a => a.AssignedAssistantId == assistantId.Value);
    }

    if (excludeAppointmentId.HasValue)
    {
      Query.Where(a => a.Id != excludeAppointmentId.Value);
    }
  }
}