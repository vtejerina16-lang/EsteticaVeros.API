namespace EsteticaVeros.API.Web.Services;

public class CreateServiceResponse(ServiceRecord service)
{
  public ServiceRecord Service { get; set; } = service;
}