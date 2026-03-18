namespace ProductClientHub.Domain.Repositories.Product;

public interface IDeleteProductOnlyRepository
{
    Task<bool> Delete(Guid clientId, Guid productId);
}
