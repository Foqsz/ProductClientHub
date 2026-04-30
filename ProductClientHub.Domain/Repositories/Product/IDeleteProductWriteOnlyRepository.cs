namespace ProductClientHub.Domain.Repositories.Product;

public interface IDeleteProductWriteOnlyRepository
{
    Task<bool> Delete(Guid clientId, Guid productId);
}
