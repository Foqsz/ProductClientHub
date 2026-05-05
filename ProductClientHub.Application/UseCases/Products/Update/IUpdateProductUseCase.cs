using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Products.Update;

public interface IUpdateProductUseCase
{
    Task<ResponseShortProductJson> Execute(Guid productId, RequestProductJson request);
}
