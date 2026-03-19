namespace ProductClientHub.Domain.Repositories.Client;

public interface IClientReadOnlyRepository
{
    Task<bool> EmailAlreadyExists(string email);
    Task<IList<Entities.Client>> GetAll();
    Task<Entities.Client> GetById(Guid clientId);
}
