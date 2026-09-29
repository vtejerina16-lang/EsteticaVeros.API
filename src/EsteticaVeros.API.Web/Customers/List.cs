using EsteticaVeros.API.UseCases.Customers;
using EsteticaVeros.API.UseCases.Customers.List;
using EsteticaVeros.API.Web.Configurations;

namespace EsteticaVeros.API.Web.Customers;

public class List(IMediator mediator)
  : Endpoint<ListCustomersRequest, CustomersListResponse, ListCustomersMapper>
{
  public override void Configure()
  {
    Get("/api/customers");
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Lista los clientes";
      s.Description = "Obtiene una lista paginada de clientes, con opciones de filtrado por estado y término de búsqueda.";
      s.ExampleRequest = new ListCustomersRequest {
        Page = 1,
        PerPage = 10,
        IsActive = true,
        SearchTerm = "María"
      };
      
      s.Params["page"] = "1-based page index (default: 1)";
      s.Params["per_page"] = "Page size 1–100 (default: 10)";
      s.Params["is_active"] = "Filter by active status";
      s.Params["search"] = "Search by name, phone, or email";

      s.Responses[200] = "Lista de clientes obtenida exitosamente";
      s.Responses[400] = "Parámetros de paginación inválidos";
      s.Responses[401] = "No autenticado";
    });

    Tags("Customers");

    Description(builder => builder
      .Accepts<ListCustomersRequest>()
      .Produces<CustomersListResponse>(200, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401));
  }

  public override async Task HandleAsync(ListCustomersRequest req, CancellationToken ct)
  {
    var query = new ListCustomersQuery(
      req.Page,
      req.PerPage,
      req.IsActive,
      req.SearchTerm
    );

    var result = await mediator.Send(query, ct);

    if (!result.IsSuccess)
    {
      await Send.ErrorsAsync(statusCode: 400, ct);
      return;
    }

    var response = Map.FromEntity(result.Value);
    await Send.OkAsync(response, ct);
  }
}