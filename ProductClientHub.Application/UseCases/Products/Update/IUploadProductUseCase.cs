using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Products.Update;

public interface IUploadProductUseCase
{
    Task<ResponseShortProductJson> Execute(Guid clientId, Guid productId, RequestProductJson request);
}
