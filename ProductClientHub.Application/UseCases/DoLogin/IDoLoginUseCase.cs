using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.DoLogin;

public interface IDoLoginUseCase
{
    Task<ResponseTokenJson> Execute(RequestLoginJson request);
}
