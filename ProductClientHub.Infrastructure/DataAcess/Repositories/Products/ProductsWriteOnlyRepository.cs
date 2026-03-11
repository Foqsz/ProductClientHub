using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories.Products;

public class ProductsWriteOnlyRepository : IProductsWriteOnlyRepository
{
    private readonly ProductClientHubDbContext _dbContext;

    public ProductsWriteOnlyRepository(ProductClientHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Product product)
    {
        await _dbContext.Products.AddAsync(product);
    }
}
