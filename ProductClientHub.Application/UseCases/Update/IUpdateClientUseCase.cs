using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Update;

public interface IUpdateClientUseCase
{
    Task<ResponseClientUpdatedJson> Execute(Guid clientId, RequestClientJson request);
}
