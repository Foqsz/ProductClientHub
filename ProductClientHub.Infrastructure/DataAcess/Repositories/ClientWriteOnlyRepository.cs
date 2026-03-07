using ProductClientHub.Domain.Repositories.Client.Register;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories;

public class ClientWriteOnlyRepository : IClientWriteOnlyRepository
{
    private readonly ProductClientHubDbContext _dbContext;

    public ClientWriteOnlyRepository(ProductClientHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Domain.Entities.Client client)
    {
        await _dbContext.Clients.AddAsync(client);
    }
}
