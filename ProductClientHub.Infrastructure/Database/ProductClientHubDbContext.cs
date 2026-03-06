using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;

namespace ProductClientHub.Infrastructure.Database;

public class ProductClientHubDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Client> Clients { get; set; }

    public ProductClientHubDbContext(DbContextOptions<ProductClientHubDbContext> options) : base(options)
    { }
}
