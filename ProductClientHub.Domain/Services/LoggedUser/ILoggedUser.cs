using ProductClientHub.Domain.Entities;

namespace ProductClientHub.Domain.Services.loggedClient;

public interface ILoggedClient
{
    public Task<Client> User();
}
