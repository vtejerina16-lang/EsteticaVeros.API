namespace EsteticaVeros.API.Core.AppointmentAggregate.Specifications;

public sealed class AppointmentsByCustomerSpec : Specification<Appointment>
{
  public AppointmentsByCustomerSpec(int customerId, bool includeServices = false)
  {
    Query.Where(a => a.CustomerId == customerId);

    if (includeServices)
    {
      Query.Include(a => a.AppointmentServices);
    }

    Query.OrderByDescending(a => a.AppointmentDate);
  }
}