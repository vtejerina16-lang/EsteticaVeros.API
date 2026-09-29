using EsteticaVeros.API.UseCases.Customers;

namespace EsteticaVeros.API.Web.Customers;

public sealed class ListCustomersMapper
  : Mapper<ListCustomersRequest, CustomersListResponse, EsteticaVeros.API.UseCases.PagedResult<CustomerDTO>>
{
  public override CustomersListResponse FromEntity(EsteticaVeros.API.UseCases.PagedResult<CustomerDTO> e)
  {
    var customers = e.Items
      .Select(c => new CustomerRecord(
        c.Id,
        c.CustomerNumber,
        c.FullName,
        c.PhoneNumber,
        c.Email,
        c.LoyaltyPoints,
        c.IsActive
      ))
      .ToList();

    return new CustomersListResponse(customers, e.Page, e.PerPage, e.TotalCount, e.TotalPages);
  }
}