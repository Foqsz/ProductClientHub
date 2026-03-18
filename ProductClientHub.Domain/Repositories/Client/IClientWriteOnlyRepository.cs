namespace ProductClientHub.Domain.Repositories.Client;

public interface IClientWriteOnlyRepository
{
    Task Add(Entities.Client client);
    Task<Entities.Client?> Update(Entities.Client client);
}
