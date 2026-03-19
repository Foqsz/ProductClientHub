using ProductClientHub.Communication.Requests;

namespace ProductClientHub.Application.UseCases.Clients.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(Guid clientId, RequestChangePassword request);
}
