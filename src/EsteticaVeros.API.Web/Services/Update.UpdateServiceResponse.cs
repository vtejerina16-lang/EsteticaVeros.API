namespace EsteticaVeros.API.Web.Services;

public class UpdateServiceResponse(ServiceDetailResponse service)
{
  public ServiceDetailResponse Service { get; set; } = service;
}