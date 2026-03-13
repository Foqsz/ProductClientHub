using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories.Clients;

public class ClientReadOnlyRepository : IClientReadOnlyRepository
{
    private readonly ProductClientHubDbContext _context;

    public ClientReadOnlyRepository(ProductClientHubDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailAlreadyExists(string email)
    {
        return await _context.Clients.AnyAsync(c => c.Email == email);
    }

    public async Task<IList<Client>> GetAll()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client> GetById(Guid clientId)
    {
        var client = await _context.Clients
            .Where(client => client.Id == clientId)
            .Include(client => client.Products)
            .FirstOrDefaultAsync();

        return client!;
    }
}
