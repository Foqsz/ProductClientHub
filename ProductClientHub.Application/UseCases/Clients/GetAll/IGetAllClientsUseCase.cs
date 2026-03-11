using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Clients.GetAll;

public interface IGetAllClientsUseCase
{
    Task<ResponseAllClientsJson> Execute();
}
