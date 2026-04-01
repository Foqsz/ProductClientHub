using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Users.Update;

public interface IUpdateClientUseCase
{
    Task<ResponseClientUpdatedJson> Execute(Guid clientId, RequestShortClientJson request);
}
