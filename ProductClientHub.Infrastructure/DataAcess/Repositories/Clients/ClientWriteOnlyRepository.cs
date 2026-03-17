using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories.Users;

public class ClientWriteOnlyRepository : IClientWriteOnlyRepository, IDeleteClientRepository
{
    private readonly ProductClientHubDbContext _dbContext;

    public ClientWriteOnlyRepository(ProductClientHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Domain.Entities.Client client)
    {
        await _dbContext.Users.AddAsync(client);
    }

    public async Task Delete(Guid clientId)
    {
        var client = await _dbContext.Users.Where(client => client.Id == clientId).FirstOrDefaultAsync();
        _dbContext.Users.Remove(client!);
    }

    public async Task<Client> Update(Client client)
    {
        var clientDb = await _dbContext.Users.FindAsync(client.Id);
        _dbContext.Users.Update(clientDb!);

        return clientDb!;
    }
}
