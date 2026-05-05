namespace ProductClientHub.Domain.Repositories.Product;

public interface IUpdateProductOnlyRepository
{
    Task Update(Guid clientId, Guid productId, Domain.Entities.Product product);
}
