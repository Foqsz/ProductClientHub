using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Clients.Register;

public interface IRegisterClientUseCase
{
    Task<ResponseShortClientJson> Execute(RequestClientJson request);
}
