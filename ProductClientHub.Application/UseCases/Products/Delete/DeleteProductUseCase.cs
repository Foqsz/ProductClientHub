using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Services.LoggedUser;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.Delete;

public class DeleteProductUseCase : IDeleteProductUseCase
{
    private readonly IDeleteProductOnlyRepository _productsWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteProductUseCase(IDeleteProductOnlyRepository productsWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _productsWriteOnlyRepository = productsWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute(Guid productId)
    {
        var client = await _loggedUser.User();

        var productExist = await _productsWriteOnlyRepository.Delete(client.Id, productId);

        if(productExist.IsFalse())
            throw new NoContentException(ResourceMessagesExceptions.PRODUCT_INVALID);

        await _unitOfWork.Commit();
    }
}
