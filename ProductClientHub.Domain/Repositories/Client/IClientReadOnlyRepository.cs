namespace ProductClientHub.Domain.Repositories.Client;

public interface IClientReadOnlyRepository
{
    Task<Entities.Client?> EmailAlreadyExists(string email);
    Task<IList<Entities.Client>> GetAll();
    Task<Entities.Client?> GetById(Guid clientId);
}
