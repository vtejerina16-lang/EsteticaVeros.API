namespace EsteticaVeros.API.Core.AppointmentAggregate.Specifications;

public sealed class AppointmentsNeedingReminderSpec : Specification<Appointment>
{
  public AppointmentsNeedingReminderSpec(int reminderHoursBefore)
  {
    var now = DateTime.UtcNow;
    var reminderTime = now.AddHours(reminderHoursBefore);

    Query.Where(a =>
      !a.ReminderSent &&
      a.AppointmentDate <= reminderTime &&
      a.AppointmentDate > now &&
      (a.Status == AppointmentStatus.Scheduled || a.Status == AppointmentStatus.Confirmed));

    Query.OrderBy(a => a.AppointmentDate);
  }
}