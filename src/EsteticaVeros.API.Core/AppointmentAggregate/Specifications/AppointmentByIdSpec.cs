namespace EsteticaVeros.API.Core.AppointmentAggregate.Specifications;

public sealed class AppointmentByIdSpec : Specification<Appointment>, ISingleResultSpecification<Appointment>
{
  public AppointmentByIdSpec(int appointmentId, bool includeServices = false)
  {
    Query.Where(a => a.Id == appointmentId);

    if (includeServices)
    {
      Query.Include(a => a.AppointmentServices);
    }
  }
}