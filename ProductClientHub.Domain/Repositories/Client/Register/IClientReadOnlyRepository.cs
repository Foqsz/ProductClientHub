
namespace ProductClientHub.Domain.Repositories.Client.Register;

public interface IClientReadOnlyRepository
{
    Task<bool> EmailAlreadyExists(string email);
}
