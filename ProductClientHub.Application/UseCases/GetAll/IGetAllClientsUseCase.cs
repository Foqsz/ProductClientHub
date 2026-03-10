using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.GetAll;

public interface IGetAllClientsUseCase
{
    Task<ResponseAllClientsJson> Execute();
}
