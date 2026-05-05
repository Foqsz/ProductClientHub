using Mapster;
using ProductClientHub.Application.UseCases.Products.SharedValidator;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Services.loggedClient;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.Update;

public class UpdateProductUseCase : IUpdateProductUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IProductsReadOnlyRepository _productsReadOnlyRepository;
    private readonly IUpdateProductOnlyRepository _productWriteOnlyRepository;
    private readonly ILoggedClient _loggedClient;

    public UpdateProductUseCase(IUnitOfWork unitOfWork,
        IUpdateProductOnlyRepository productWriteOnlyRepository,
        IClientReadOnlyRepository clientReadOnlyRepository,
        IProductsReadOnlyRepository productsReadOnlyRepository,
        ILoggedClient loggedClient)
    {
        _unitOfWork = unitOfWork;
        _productWriteOnlyRepository = productWriteOnlyRepository;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _productsReadOnlyRepository = productsReadOnlyRepository;
        _loggedClient = loggedClient;
    }

    public async Task<ResponseShortProductJson> Execute(Guid productId, RequestProductJson request)
    {
        var client = await _loggedClient.User();
        await Validate(client.Id, request);

        var product = await _productsReadOnlyRepository.GetById(productId) ?? throw new NotFoundException(ResourceMessagesExceptions.PRODUCT_NOTFOUND);

        product.Name = request.Name;
        product.Brand = request.Brand;
        product.Price = request.Price;

        await _productWriteOnlyRepository.Update(client.Id, productId, product);
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
