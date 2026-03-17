using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Users.GetAll;

public interface IGetAllClientsUseCase
{
    Task<ResponseAllClientsJson> Execute();
}
