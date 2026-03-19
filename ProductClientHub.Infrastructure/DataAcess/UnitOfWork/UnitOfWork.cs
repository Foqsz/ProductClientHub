using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductClientHubDbContext _dbContext;

    public UnitOfWork(ProductClientHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}
