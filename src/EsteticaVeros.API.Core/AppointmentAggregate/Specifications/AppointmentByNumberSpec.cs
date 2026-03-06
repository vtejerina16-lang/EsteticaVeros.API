namespace EsteticaVeros.API.Core.AppointmentAggregate.Specifications;

public sealed class AppointmentByNumberSpec : Specification<Appointment>, ISingleResultSpecification<Appointment>
{
  public AppointmentByNumberSpec(AppointmentNumber appointmentNumber, bool includeServices = false)
  {
    Query.Where(a => a.AppointmentNumber == appointmentNumber);

    if (includeServices)
    {
      Query.Include(a => a.AppointmentServices);
    }
  }
}