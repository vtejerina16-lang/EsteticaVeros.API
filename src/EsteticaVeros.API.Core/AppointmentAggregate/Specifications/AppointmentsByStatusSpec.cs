namespace EsteticaVeros.API.Core.AppointmentAggregate.Specifications;

public sealed class AppointmentsByStatusSpec : Specification<Appointment>
{
  public AppointmentsByStatusSpec(AppointmentStatus status, bool includeServices = false)
  {
    Query.Where(a => a.Status == status);

    if (includeServices)
    {
      Query.Include(a => a.AppointmentServices);
    }

    Query.OrderBy(a => a.AppointmentDate);
  }
}