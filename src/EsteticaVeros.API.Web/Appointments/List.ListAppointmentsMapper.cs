using EsteticaVeros.API.UseCases.Appointments;

namespace EsteticaVeros.API.Web.Appointments;

public sealed class ListAppointmentsMapper
  : Mapper<ListAppointmentsRequest, AppointmentsListResponse, List<AppointmentDTO>>
{
  public override AppointmentsListResponse FromEntity(List<AppointmentDTO> e)
  {
    var appointments = e
      .Select(a => new AppointmentRecord(
        a.Id,
        a.AppointmentNumber,
        a.CustomerName,
        a.AppointmentDate,
        a.EstimatedDuration,
        a.StatusName,
        a.TotalAmount
      ))
      .ToList();

    return new AppointmentsListResponse(appointments);
  }
}