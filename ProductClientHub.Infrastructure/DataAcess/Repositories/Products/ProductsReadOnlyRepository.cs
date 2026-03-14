using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories.Products;

public class ProductsReadOnlyRepository : IProductsReadOnlyRepository
{
    private readonly ProductClientHubDbContext _dbContext;

    public ProductsReadOnlyRepository(ProductClientHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Product>> GetAll()
    {
        return await _dbContext.Products
            .Include(p => p.Client)
            .ToListAsync();
    }
}
