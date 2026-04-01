using Moq;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Client;

namespace CommonTestUtilities.Repositories;

public class ClientReadOnlyRepositoryBuilder
{
    private readonly Mock<IClientReadOnlyRepository> _repository;

    public ClientReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IClientReadOnlyRepository>();
    }

    public ClientReadOnlyRepositoryBuilder EmailAlreadyExists(Client? client)
    {
        if(client is not null)
            _repository.Setup(r => r.EmailAlreadyExists(client.Email)).ReturnsAsync(client);

        return this;
    }

    public ClientReadOnlyRepositoryBuilder GetAll(IList<Client> clients)
    {
        _repository.Setup(r => r.GetAll()).ReturnsAsync(clients);
        return this;
    }

    public ClientReadOnlyRepositoryBuilder GetById(Client? client)
    {
        if(client is not null)
            _repository.Setup(r => r.GetById(client.Id)).ReturnsAsync(client);
        return this;
    }

    public ClientReadOnlyRepositoryBuilder ExistActiveClientWithIdentifier(Guid clientIdentifier, bool exist)
    {
        _repository.Setup(r => r.ExistActiveClientWithIdentifier(clientIdentifier)).ReturnsAsync(exist);
        return this;
    }

    public IClientReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
