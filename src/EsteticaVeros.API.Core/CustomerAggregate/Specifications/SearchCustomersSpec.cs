namespace EsteticaVeros.API.Core.CustomerAggregate.Specifications;

public sealed class SearchCustomersSpec : Specification<Customer>
{
  public SearchCustomersSpec(string searchTerm, bool activeOnly = true)
  {
    var search = searchTerm.ToLower();
    var normalizedPhone = new string(searchTerm.Where(char.IsDigit).ToArray());

    Query.Where(c =>
      c.FirstName.ToLower().Contains(search) ||
      c.LastName.ToLower().Contains(search) ||
      (c.Email != null && c.Email.ToLower().Contains(search)) ||
      (normalizedPhone.Length > 0 && c.PhoneNumber != null && c.PhoneNumber.Contains(normalizedPhone))
    );

    if (activeOnly)
    {
      Query.Where(c => c.IsActive);
    }

    Query
      .OrderBy(c => c.FirstName)
      .ThenBy(c => c.LastName);
  }
}