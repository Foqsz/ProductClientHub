using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Products.Register;

public interface IRegisterProductUseCase
{
    Task<ResponseProductJson> Execute(Guid clientId, RequestProductJson request);
}
