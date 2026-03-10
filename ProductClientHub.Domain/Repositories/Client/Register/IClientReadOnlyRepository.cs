
namespace ProductClientHub.Domain.Repositories.Client.Register;

public interface IClientReadOnlyRepository
{
    Task<bool> EmailAlreadyExists(string email);
    Task<IList<Entities.Client>> GetAll();
}
