using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.Delete;

public class DeleteProductUseCase : IDeleteProductUseCase
{
    private readonly IDeleteProductOnlyRepository _productsWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductUseCase(IDeleteProductOnlyRepository productsWriteOnlyRepository, 
        IUnitOfWork unitOfWork)
    {
        _productsWriteOnlyRepository = productsWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid clientId, Guid productId)
    {
        var getProduct = await _productsWriteOnlyRepository.Delete(clientId, productId);

        if(getProduct.IsFalse())
            throw new NoContentException(ResourceMessagesExceptions.PRODUCT_INVALID);

        await _unitOfWork.Commit();
    }
}
