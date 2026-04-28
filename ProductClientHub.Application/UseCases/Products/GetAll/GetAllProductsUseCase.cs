using Mapster;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Services.loggedClient;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.GetAll;

public class GetAllProductsUseCase : IGetAllProductsUseCase
{
    private readonly IProductsReadOnlyRepository _productReadOnlyRepository;
    private readonly ILoggedClient _loggedClient;

    public GetAllProductsUseCase(IProductsReadOnlyRepository productReadOnlyRepository, ILoggedClient loggedClient)
    {
        _productReadOnlyRepository = productReadOnlyRepository;
        _loggedClient = loggedClient;
    }

    public async Task<ResponseProductsJson> Execute()
    {
        var user = await _loggedClient.User();

        if (user is null)
            throw new UserNotLoggedException(ResourceMessagesExceptions.NOT_LOGGED);

        var products = await _productReadOnlyRepository.GetAll();

        if (products is null || products.Any().IsFalse())
            throw new NotFoundException(ResourceMessagesExceptions.PRODUCT_NOTFOUND);

        return new ResponseProductsJson
        {
            Products = products.Adapt<List<ResponseShortProductJson>>()
        };
    }
}
