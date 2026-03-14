namespace ProductClientHub.Domain.Repositories.Product;

public interface IProductsReadOnlyRepository
{
    Task<IList<Entities.Product>> GetAll();
}
