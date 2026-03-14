using Mapster;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.GetAll;

public class GetAllProductsUseCase : IGetAllProductsUseCase
{
    private readonly IProductsReadOnlyRepository _productReadOnlyRepository;

    public GetAllProductsUseCase(IProductsReadOnlyRepository productReadOnlyRepository)
    {
        _productReadOnlyRepository = productReadOnlyRepository;
    }

    public async Task<ResponseProductsJson> Execute()
    {
        var products = await _productReadOnlyRepository.GetAll();

        if (products is null || !products.Any())
            throw new NotFoundException(ResourceMessagesExceptions.PRODUCT_NOTFOUND);

        return new ResponseProductsJson
        {
            Products = products.Adapt<List<ResponseShortProductJson>>()
        };
    }
}
