using Mapster;
using ProductClientHub.Application.UseCases.Products.SharedValidator;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.Update;

public class UploadProductUseCase : IUploadProductUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IProductsReadOnlyRepository _productsReadOnlyRepository;
    private readonly IUploadProductOnlyRepository _productWriteOnlyRepository;

    public UploadProductUseCase(IUnitOfWork unitOfWork,
        IUploadProductOnlyRepository productWriteOnlyRepository,
        IClientReadOnlyRepository clientReadOnlyRepository,
        IProductsReadOnlyRepository productsReadOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _productWriteOnlyRepository = productWriteOnlyRepository;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _productsReadOnlyRepository = productsReadOnlyRepository;
    }

    public async Task<ResponseShortProductJson> Execute(Guid clientId, Guid productId, RequestProductJson request)
    {
        await Validate(clientId, request);

        var product = await _productsReadOnlyRepository.GetById(productId);

        if (product == null)
            throw new NotFoundException(ResourceMessagesExceptions.PRODUCT_NOTFOUND);

        product.Name = request.Name;
        product.Brand = request.Brand;
        product.Price = request.Price;

        await _productWriteOnlyRepository.Update(clientId, productId, product);
        await _unitOfWork.Commit();

        return product.Adapt<ResponseShortProductJson>();
    }

    private async Task Validate(Guid clientId, RequestProductJson request)
    {
        var clientExist = await _clientReadOnlyRepository.GetById(clientId);

        if (clientExist is null)
            throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);

        var validator = new RequestProductValidator();

        var validationResult = validator.Validate(request);

        if (validationResult.IsValid.IsFalse())
        {
            var errors = validationResult.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
