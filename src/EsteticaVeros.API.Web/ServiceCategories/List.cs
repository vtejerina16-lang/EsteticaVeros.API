using EsteticaVeros.API.UseCases.ServiceCategories;
using EsteticaVeros.API.UseCases.ServiceCategories.List;
using EsteticaVeros.API.Web.Configurations;

namespace EsteticaVeros.API.Web.ServiceCategories;

public class List(IMediator mediator)
  : Endpoint<ListServiceCategoriesRequest, ServiceCategoriesListResponse, ListServiceCategoriesMapper>
{
  public override void Configure()
  {
    Get("/api/service-categories");
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Lista de categorías de servicios";
      s.Description = "Obtiene una lista de categorías de servicios, con opción de filtrar por estado activo.";
      s.ExampleRequest = new ListServiceCategoriesRequest {
        IsActive = true
      };
      
      s.Params["is_active"] = "Filter by active status (optional)";

      s.Responses[200] = "Lista de categorías de servicios obtenida exitosamente";
      s.Responses[401] = "No autenticado";
    });

    Tags("Service Categories");

    Description(builder => builder
      .Accepts<ListServiceCategoriesRequest>()
      .Produces<ServiceCategoriesListResponse>(200, "application/json")
      .ProducesProblem(401));
  }

  public override async Task HandleAsync(ListServiceCategoriesRequest req, CancellationToken ct)
  {
    var query = new ListServiceCategoriesQuery(req.IsActive);
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