using EsteticaVeros.API.UseCases.Services;
using EsteticaVeros.API.UseCases.Services.List;
using EsteticaVeros.API.Web.Configurations;

namespace EsteticaVeros.API.Web.Services;

public class List(IMediator mediator)
  : Endpoint<ListServicesRequest, ServicesListResponse, ListServicesMapper>
{
  public override void Configure()
  {
    Get("/api/services");
    Policies(AuthorizationConfig.RequireAuthenticatedUser);

    Summary(s =>
    {
      s.Summary = "Lista los servicios";
      s.Description = "Obtiene una lista de servicios, con opciones de filtrado por tipo, categoría, estado y destacado.";
      s.ExampleRequest = new ListServicesRequest {
        ServiceType = 1,
        CategoryId = 2,
        IsActive = true,
        IsFeatured = false
      };
      
      s.Params["service_type"] = "Filter by service type (1=Simple, 2=Composite, 3=Package)";
      s.Params["category_id"] = "Filter by category ID";
      s.Params["is_active"] = "Filter by active status";
      s.Params["is_featured"] = "Filter by featured status";

      s.Responses[200] = "Lista de servicios obtenida exitosamente";
      s.Responses[401] = "No autenticado";
    });

    Tags("Services");

    Description(builder => builder
      .Accepts<ListServicesRequest>()
      .Produces<ServicesListResponse>(200, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(401));
  }

  public override async Task HandleAsync(ListServicesRequest req, CancellationToken ct)
  {
    var query = new ListServicesQuery(
      req.ServiceType,
      req.CategoryId,
      req.IsActive,
      req.IsFeatured
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