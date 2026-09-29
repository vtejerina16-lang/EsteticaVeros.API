namespace EsteticaVeros.API.Core.AppointmentAggregate.Specifications;

public sealed class UpcomingAppointmentsSpec : Specification<Appointment>
{
  public UpcomingAppointmentsSpec(int? customerId = null, bool includeServices = false)
  {
    var now = DateTime.UtcNow;

    Query.Where(a => 
      a.AppointmentDate >= now &&
      (a.Status == AppointmentStatus.Scheduled || a.Status == AppointmentStatus.Confirmed));

    if (customerId.HasValue)
    {
      Query.Where(a => a.CustomerId == customerId.Value);
    }

    if (includeServices)
    {
      Query.Include(a => a.AppointmentServices);
    }

    Query.OrderBy(a => a.AppointmentDate);
  }
}