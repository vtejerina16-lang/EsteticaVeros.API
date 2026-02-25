using Ardalis.Specification;
using EsteticaVeros.API.Core.CustomerAggregate;
using EsteticaVeros.API.Core.CustomerAggregate.Specifications;

namespace EsteticaVeros.API.UseCases.Customers.List;

public class ListCustomersHandler(
  IRepository<Customer> repository,
  ILogger<ListCustomersHandler> logger) : IQueryHandler<ListCustomersQuery, Result<PagedResult<CustomerDTO>>>
{
  public async ValueTask<Result<PagedResult<CustomerDTO>>> Handle(
    ListCustomersQuery query,
    CancellationToken cancellationToken)
  {
    try
    {
      logger.LogInformation(
        "Listing customers - Page: {Page}, PerPage: {PerPage}, IsActive: {IsActive}, Search: {Search}",
        query.Page, query.PerPage, query.IsActive, query.SearchTerm);

      if (!string.IsNullOrWhiteSpace(query.SearchTerm))
      {
        return await SearchCustomersInMemory(query, cancellationToken);
      }

      return await ListCustomersFromDb(query, cancellationToken);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error listing customers with query: {@Query}", query);
      return Result.Error($"Error retrieving customers: {ex.Message}");
    }
  }

  private async Task<Result<PagedResult<CustomerDTO>>> ListCustomersFromDb(
    ListCustomersQuery query,
    CancellationToken cancellationToken)
  {
    Specification<Customer> spec = new Specification<Customer>();

    if (query.IsActive.HasValue)
    {
      spec.Query.Where(c => c.IsActive == query.IsActive.Value);
    }

    spec.Query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);

    var totalCount = await repository.CountAsync(spec, cancellationToken);
    spec.Query.Skip((query.Page - 1) * query.PerPage).Take(query.PerPage);
    var customers = await repository.ListAsync(spec, cancellationToken);

    var customerDtos = customers.Select(MapToDTO).ToList();
    var totalPages = (int)Math.Ceiling(totalCount / (double)query.PerPage);

    logger.LogInformation("Listed {Count} customers from DB", totalCount);

    return Result.Success(new PagedResult<CustomerDTO>(
      customerDtos, query.Page, query.PerPage, totalCount, totalPages));
  }

  private async Task<Result<PagedResult<CustomerDTO>>> SearchCustomersInMemory(
    ListCustomersQuery query,
    CancellationToken cancellationToken)
  {
    var search = query.SearchTerm!.ToLower().Trim();
    var searchUpper = query.SearchTerm!.ToUpper().Trim();
    var phoneDigits = new string(query.SearchTerm.Where(char.IsDigit).ToArray());

    logger.LogInformation(
      "Searching in memory - Term: {Term}, Phone: {Phone}", 
      search, phoneDigits);

    // Traer TODOS los customers (o filtrados por IsActive si aplica)
    Specification<Customer> spec = new Specification<Customer>();
    
    if (query.IsActive.HasValue)
    {
      spec.Query.Where(c => c.IsActive == query.IsActive.Value);
    }

    var allCustomers = await repository.ListAsync(spec, cancellationToken);

    logger.LogInformation("Retrieved {Count} customers from DB for in-memory filtering", allCustomers.Count);

    // Filtrar EN MEMORIA (aquí sí podemos usar CustomerNumber.Value)
    var filtered = allCustomers.Where(c =>
      // Nombre
      c.FirstName.ToLower().Contains(search) ||
      c.LastName.ToLower().Contains(search) ||
      c.FullName.ToLower().Contains(search) ||
      // Email
      (c.Email != null && c.Email.ToLower().Contains(search)) ||
      // Teléfono (solo dígitos)
      (phoneDigits.Length >= 3 && c.PhoneNumber.Contains(phoneDigits)) ||
      // CustomerNumber (busca coincidencias parciales: "2024", "CL-2024", "0001", etc.)
      c.CustomerNumber.Value.Contains(searchUpper)
    ).ToList();

    logger.LogInformation("Found {Count} customers matching search term", filtered.Count);

    // Ordenar
    var ordered = filtered
      .OrderBy(c => c.FirstName)
      .ThenBy(c => c.LastName)
      .ToList();

    var totalCount = ordered.Count;
    var totalPages = (int)Math.Ceiling(totalCount / (double)query.PerPage);

    // Paginar
    var paged = ordered
      .Skip((query.Page - 1) * query.PerPage)
      .Take(query.PerPage)
      .ToList();

    var customerDtos = paged.Select(MapToDTO).ToList();

    logger.LogInformation(
      "Returning page {Page} of {TotalPages} with {Count} customers", 
      query.Page, totalPages, customerDtos.Count);

    return Result.Success(new PagedResult<CustomerDTO>(
      customerDtos, query.Page, query.PerPage, totalCount, totalPages));
  }

  private static CustomerDTO MapToDTO(Customer customer)
  {
    return new CustomerDTO(
      customer.Id,
      customer.CustomerNumber.Value,
      customer.FirstName,
      customer.LastName,
      customer.FullName,
      customer.PhoneNumber,
      customer.Email,
      customer.DateOfBirth,
      customer.Age,
      customer.Gender.Name,
      customer.Address,
      customer.City,
      customer.Allergies,
      customer.MedicalConditions,
      customer.Medications,
      customer.PreferredAssistantId,
      customer.ProfilePictureUrl,
      customer.LoyaltyPoints,
      customer.TotalSpent,
      customer.TotalVisits,
      customer.LastVisitDate,
      customer.IsActive,
      customer.CreatedAt,
      customer.UpdatedAt
    );
  }
}