using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.UseCases.Users.GetById;

public interface IGetClientByIdUseCase
{
    Task<ResponseClientJson> Execute(Guid clientId);
}
