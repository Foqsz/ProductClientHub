namespace ProductClientHub.Application.UseCases.Products.Delete;

public interface IDeleteProductUseCase
{
    Task Execute(Guid clientId, Guid productId);
}
