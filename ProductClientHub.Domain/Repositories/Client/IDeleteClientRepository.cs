namespace ProductClientHub.Domain.Repositories.Client;

public interface IDeleteClientRepository
{
    Task Delete(Guid clientId);
}
