namespace ProductClientHub.Domain.Repositories.Client.Register;

public interface IClientWriteOnlyRepository
{
    Task Add(Entities.Client client);
}
