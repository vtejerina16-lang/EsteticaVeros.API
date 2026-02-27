namespace EsteticaVeros.API.UseCases.ServiceCategories.Delete;

public record DeleteServiceCategoryCommand(int CategoryId) : ICommand<Result>;