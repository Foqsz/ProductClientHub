namespace ProductClientHub.Domain.Repositories.Product;

public interface IUploadProductOnlyRepository
{
    Task Update(Guid clientId, Guid productId, Domain.Entities.Product product);
}
