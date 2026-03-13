using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories.Clients;

public class ClientWriteOnlyRepository : IClientWriteOnlyRepository, IDeleteClientRepository
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

    public async Task Delete(Guid clientId)
    {
        var client = await _dbContext.Clients.Where(client => client.Id == clientId).FirstOrDefaultAsync();
        _dbContext.Clients.Remove(client!);
    }

    public async Task<Client> Update(Client client)
    {
        var clientDb = await _dbContext.Clients.FindAsync(client.Id);
        _dbContext.Clients.Update(clientDb!);

        return clientDb!;
    }
}
