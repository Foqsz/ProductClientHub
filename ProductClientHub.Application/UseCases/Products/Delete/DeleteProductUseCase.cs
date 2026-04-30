using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Services.loggedClient;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.Delete;

public class DeleteProductUseCase : IDeleteProductUseCase
{
    private readonly IDeleteProductOnlyRepository _productsWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedClient _loggedClient;

    public DeleteProductUseCase(IDeleteProductOnlyRepository productsWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        ILoggedClient loggedClient)
    {
        _productsWriteOnlyRepository = productsWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedClient = loggedClient;
    }

    public async Task Execute(Guid productId)
    {
        var client = await _loggedClient.User();

        var productExist = await _productsWriteOnlyRepository.Delete(client.Id, productId);

        if(productExist.IsFalse())
            throw new NoContentException(ResourceMessagesExceptions.PRODUCT_INVALID);

        await _unitOfWork.Commit();
    }
}
