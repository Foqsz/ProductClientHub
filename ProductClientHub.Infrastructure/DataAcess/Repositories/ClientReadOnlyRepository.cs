using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Client.Register;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories;

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
}
