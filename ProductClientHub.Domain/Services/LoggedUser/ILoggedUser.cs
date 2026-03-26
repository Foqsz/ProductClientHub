using ProductClientHub.Domain.Entities;

namespace ProductClientHub.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<Client> User();
}
