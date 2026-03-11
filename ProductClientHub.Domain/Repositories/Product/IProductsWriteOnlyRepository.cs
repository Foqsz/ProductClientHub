namespace ProductClientHub.Domain.Repositories.Product;

public interface IProductsWriteOnlyRepository
{
    Task Add(Entities.Product product);
}
