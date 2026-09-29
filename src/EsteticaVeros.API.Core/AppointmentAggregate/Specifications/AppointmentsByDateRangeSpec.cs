namespace EsteticaVeros.API.Core.AppointmentAggregate.Specifications;

public sealed class AppointmentsByDateRangeSpec : Specification<Appointment>
{
  public AppointmentsByDateRangeSpec(
    DateTime startDate,
    DateTime endDate,
    int? assistantId = null,
    bool includeServices = false)
  {
    Query.Where(a => a.AppointmentDate >= startDate && a.AppointmentDate < endDate);

    if (assistantId.HasValue)
    {
      Query.Where(a => a.AssignedAssistantId == assistantId.Value);
    }

    if (includeServices)
    {
      Query.Include(a => a.AppointmentServices);
    }

    Query.OrderBy(a => a.AppointmentDate);
  }
}