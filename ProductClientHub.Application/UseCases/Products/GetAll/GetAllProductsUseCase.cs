using Mapster;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Services.LoggedUser;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.GetAll;

public class GetAllProductsUseCase : IGetAllProductsUseCase
{
    private readonly IProductsReadOnlyRepository _productReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public GetAllProductsUseCase(IProductsReadOnlyRepository productReadOnlyRepository, ILoggedUser loggedUser)
    {
        _productReadOnlyRepository = productReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseProductsJson> Execute()
    {
        var user = await _loggedUser.User();

        if(user is null)
            throw new UserNotLoggedException(ResourceMessagesExceptions.NOT_LOGGED);

        var products = await _productReadOnlyRepository.GetAll();

        if (products is null || !products.Any())
            throw new NotFoundException(ResourceMessagesExceptions.PRODUCT_NOTFOUND);

        return new ResponseProductsJson
        {
            Products = products.Adapt<List<ResponseShortProductJson>>()
        };
    }
}
