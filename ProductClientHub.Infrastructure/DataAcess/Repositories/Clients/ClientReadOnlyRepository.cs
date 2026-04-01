using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories.Users;

public class ClientReadOnlyRepository : IClientReadOnlyRepository
{
    private readonly ProductClientHubDbContext _context;

    public ClientReadOnlyRepository(ProductClientHubDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> EmailAlreadyExists(string email)
    {
        return await _context.Users.Where(c => c.Email == email).FirstOrDefaultAsync();
    }

    public async Task<bool> ExistActiveClientWithIdentifier(Guid clientIdentifier)
    {
        return await _context.Users.Where(c => c.Id == clientIdentifier && c.Active).AnyAsync();
    }

    public async Task<IList<Client>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Client?> GetById(Guid clientId)
    {
        return await _context.Users
            .Where(client => client.Id == clientId)
            .Include(client => client.Products)
            .FirstOrDefaultAsync();
    }
}
