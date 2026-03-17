using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.GetById;

public interface IGetProductByIdUseCase
{
    Task<ResponseShortProductJson> Execute(Guid productId);
}
