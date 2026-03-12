using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Clients.GetById;

public interface IGetClientByIdUseCase
{
    Task<ResponseClientJson> Execute(Guid clientId);
}
