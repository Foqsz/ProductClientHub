using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories.Products;

public class ProductsWriteOnlyRepository : IProductsWriteOnlyRepository, IDeleteProductOnlyRepository, IUploadProductOnlyRepository
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

    public async Task<bool> Delete(Guid clientId, Guid productId)
    {
        var product = await _dbContext.Products.Where(p => p.ClientId == clientId && p.Id == productId).FirstOrDefaultAsync();

        if (product is null)
            return false;

        _dbContext.Products.Remove(product);
        return true;
    }

    public async Task Update(Guid clientId, Guid productId, Product product)
    {
        var productUpload = await _dbContext.Products.Where(p => p.ClientId == clientId && p.Id == productId).FirstOrDefaultAsync();

        if (productUpload is null) return;

        var createdOn = await _dbContext.Products
            .Where(x => x.Id == product.Id)
            .Select(x => x.CreatedOn)
            .FirstOrDefaultAsync();

        product.CreatedOn = DateTime.SpecifyKind(createdOn, DateTimeKind.Utc);

        _dbContext.Products.Update(productUpload);

        _dbContext.Entry(product).Property(x => x.CreatedOn).IsModified = false;
    }
}
