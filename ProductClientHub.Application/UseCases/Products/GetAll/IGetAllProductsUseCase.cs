using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Products.GetAll;

public interface IGetAllProductsUseCase
{
    Task<ResponseProductsJson> Execute();
}
